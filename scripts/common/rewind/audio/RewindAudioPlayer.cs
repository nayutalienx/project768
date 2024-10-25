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
    [Export] public Label AudioLabel;
    [Export] public AudioStreamPlayer ForwardPlayer { get; set; }
    [Export] public AudioStreamPlayer BackwardPlayer { get; set; }

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
        audioLen = ForwardPlayer.GetStream().GetLength();

        ForwardPlayer.Play();

        StateChanger.ChangeState(State.Forward);
    }

    public override void _Process(double delta)
    {
        if (AudioLabel != null)
        {
            AudioLabel.Text = $"{CurrentState.StateEnum}\n" +
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
        BackwardPlayer.Play((float) backwardPos);
    }

    public void SyncForwardFromBackward()
    {
        double forwardPos = audioLen - BackwardPlayer.GetPlaybackPosition();
        GD.Print($"forwardPos {forwardPos} = {audioLen} - {BackwardPlayer.GetPlaybackPosition()}");
        ForwardPlayer.Play((float) forwardPos);
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