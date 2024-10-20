using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.cannon;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using MoveState = project768.scripts.platform.MoveState;
using RewindState = project768.scripts.platform.RewindState;

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
    public Dictionary<State, State<OneWayPlatform, State>> States { get; set; }
    public StateChanger<OneWayPlatform, State> StateChanger { get; set; }

    [Export] public string animationName { get; set; }

    public RewindableAnimationPlayer AnimationPlayer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        States = new Dictionary<State, State<OneWayPlatform, State>>()
        {
            {State.Move, new MoveState(this, State.Move)},
            {State.Rewind, new RewindState(this, State.Rewind)},
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
            AnimationPlayer.UpdateRewindSpeed(speed);
        }
    }
}