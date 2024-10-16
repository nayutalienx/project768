using Godot;
using System;
using project768.scripts.common;
using project768.scripts.platform;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class OneWayPlatform :
    Node2D,
    IRewindable,
    IStateMachineEntity<OneWayPlatform, OneWayPlatform.State>
{
    public enum State
    {
        Move,
        Rewind
    }

    public int RewindState { get; set; }
    public State<OneWayPlatform, State> CurrentState { get; set; }
    public State<OneWayPlatform, State>[] States { get; set; }
    public StateChanger<OneWayPlatform, State> StateChanger { get; set; }

    [Export] public string animationName { get; set; }

    public RewindableAnimationPlayer AnimationPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        States = new State<OneWayPlatform, State>[]
        {
            new MoveState(this, State.Move),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<OneWayPlatform, State>(this);
        StateChanger.ChangeState(State.Move);


        if (HasNode("AnimationPlayer"))
        {
            AnimationPlayer = new RewindableAnimationPlayer(
                GetNode<AnimationPlayer>("AnimationPlayer"),
                new[]
                {
                    "Platform1"
                });
            if (AnimationPlayer != null && animationName != null)
            {
                AnimationPlayer.Play(animationName);
            }
        }
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void OnRewindSpeedChanged(int speed)
    {
        if (AnimationPlayer != null)
        {
            AnimationPlayer.AnimationPlayer.SpeedScale = speed;
        }
    }
}