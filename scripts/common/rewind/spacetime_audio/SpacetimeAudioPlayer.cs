using System;
using System.Collections.Generic;
using Godot;
using project768.scripts.player;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.rewind.spacetime_audio;

public partial class SpacetimeAudioPlayer : Node2D,
    IRewindable,
    IStateMachineEntity<SpacetimeAudioPlayer, SpacetimeAudioPlayer.State>
{
    public enum State
    {
        Forward,
        Stopped,
        Backward
    }

    [Export] public Label AudioLabel;
    [Export] public AudioStreamPlayer ForwardPlayer { get; set; }
    [Export] public AudioStreamPlayer BackwardPlayer { get; set; }
    public State<SpacetimeAudioPlayer, State> CurrentState { get; set; }
    public Dictionary<State, State<SpacetimeAudioPlayer, State>> States { get; set; }
    public StateChanger<SpacetimeAudioPlayer, State> StateChanger { get; set; }
    public int RewindState { get; set; }
    public int RewindSpeed { get; set; }
    private double audioLen;
    private Player Player;

    private Vector2 PlayerPrevPos;
    public float PlayerPosDelta => Player.GlobalPosition.X - PlayerPrevPos.X;

    public override void _Ready()
    {
        
        States = new Dictionary<State, State<SpacetimeAudioPlayer, State>>()
        {
            {State.Forward, new ForwardState(this, State.Forward)},
            {State.Stopped, new StoppedState(this, State.Stopped)},
            {State.Backward, new BackwardState(this, State.Backward)},
        };
        StateChanger = new StateChanger<SpacetimeAudioPlayer, State>(this);
        
        Player = GetTree().GetFirstNodeInGroup("player") as Player;
        audioLen = ForwardPlayer.GetStream().GetLength();
        ForwardPlayer.Play();
        
        if (Player.PreviousSceneData.SpawnPositionIndex == 0)
        {
            StateChanger.ChangeState(State.Forward);    
        }
        else
        {
            StateChanger.ChangeState(State.Backward);
        }
    }

    public override void _Process(double delta)
    {
        if (AudioLabel != null)
        {
            AudioLabel.Text = $"PosDelta: {PlayerPosDelta}\n" +
                              $"s: {CurrentState.StateEnum}\n" +
                              $"fwd: {ForwardPlayer.GetPlaybackPosition()}\n" +
                              $"bwd: {BackwardPlayer.GetPlaybackPosition()}";
        }

        CurrentState.Process(delta);

        PlayerPrevPos = Player.GlobalPosition;
    }


    public void SyncBackwardFromForward()
    {
        double backwardPos = audioLen - ForwardPlayer.GetPlaybackPosition();
        BackwardPlayer.Play((float) backwardPos);
    }

    public void SyncForwardFromBackward()
    {
        double forwardPos = audioLen - BackwardPlayer.GetPlaybackPosition();
        ForwardPlayer.Play((float) forwardPos);
    }
    
    public void RewindStarted()
    {
        
    }

    public void RewindFinished()
    {
        
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