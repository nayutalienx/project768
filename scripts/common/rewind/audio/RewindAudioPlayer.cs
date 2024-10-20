using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.rewind;

public partial class RewindAudioPlayer :
    Node2D,
    IRewindable,
    IStateMachineEntity<RewindAudioPlayer, RewindAudioPlayer.State>
{
    [Export] public Label audioLabel;

    public enum State
    {
        Forward,
        Stopped,
        Backward
    }

    public int RewindState { get; set; }
    public State<RewindAudioPlayer, State> CurrentState { get; set; }
    public Dictionary<State, State<RewindAudioPlayer, State>> States { get; set; }
    public StateChanger<RewindAudioPlayer, State> StateChanger { get; set; }

    public AudioStreamPlayer ForwardPlayer { get; set; }
    public AudioStreamPlayer BackwardPlayer { get; set; }
    public int RewindSpeed { get; set; }

    private double audioLen;

    public override void _Ready()
    {
        States = new Dictionary<State, State<RewindAudioPlayer, State>>()
        {
            {State.Forward, new ForwardState(this, State.Forward)},
            {State.Stopped, new StoppedState(this, State.Stopped)},
            {State.Backward, new BackwardState(this, State.Backward)},
        };
        StateChanger = new StateChanger<RewindAudioPlayer, State>(this);

        ForwardPlayer = GetNode<AudioStreamPlayer>("forward_player");
        BackwardPlayer = GetNode<AudioStreamPlayer>("reverse_player");
        audioLen = ForwardPlayer.GetStream().GetLength();

        ForwardPlayer.Play();
        BackwardPlayer.Play();

        StateChanger.ChangeState(State.Forward);
    }

    public override void _Process(double delta)
    {
        if (audioLabel != null)
        {
            audioLabel.Text = $"{CurrentState.StateEnum}\n" +
                              $"fwd: {ForwardPlayer.GetPlaybackPosition()}\n" +
                              $"bwd: {BackwardPlayer.GetPlaybackPosition()}";
        }
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Backward);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState(State.Forward);
    }

    public void SyncBackwardFromForward()
    {
        double backwardPos = audioLen - ForwardPlayer.GetPlaybackPosition();
        GD.Print($"backwardPos {backwardPos} = {audioLen} - {ForwardPlayer.GetPlaybackPosition()}");
        BackwardPlayer.Seek((float) backwardPos);
    }

    public void SyncForwardFromBackward()
    {
        double forwardPos = audioLen - BackwardPlayer.GetPlaybackPosition();
        GD.Print($"forwardPos {forwardPos} = {audioLen} - {BackwardPlayer.GetPlaybackPosition()}");
        ForwardPlayer.Seek((float) forwardPos);
    }

    public void OnRewindSpeedChanged(int speed)
    {
        RewindSpeed = speed;

        if (RewindSpeed == 0)
        {
            StateChanger.ChangeState(State.Stopped);
        }
        else
        {
            ForwardPlayer.PitchScale = Math.Abs(speed);
            BackwardPlayer.PitchScale = Math.Abs(speed);

            if (RewindSpeed > 0)
            {
                StateChanger.ChangeState(State.Backward);
            }
            else
            {
                StateChanger.ChangeState(State.Forward);
            }
        }
    }
}