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
    AnimatableBody2D,
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

    [Export] public bool DisableOneWayCollission { get; set; }

    [Export] public string AnimationNameOnStart { get; set; }
    [Export] public string[] AllAnimations { get; set; }
    [Export] public bool StartAnimationOnReady { get; set; }

    [ExportSubgroup("Platform Move Mode")]
    [Export]
    public bool PlatformMoveMode { get; set; } = false;

    [Export] public Vector2 PlatformMoveVector { get; set; } = Vector2.Zero;
    [Export] public bool StopOnRaycast = false;

    public RewindableAnimationPlayer AnimationPlayer { get; set; }
    public RayCast2D RayCast2D { get; set; }

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

        RayCast2D = GetNode<RayCast2D>("RayCast2D");
        if (!StopOnRaycast)
        {
            RayCast2D.Enabled = false;
        }

        if (HasNode("AnimationPlayer"))
        {
            AnimationPlayer = new RewindableAnimationPlayer(
                GetNode<AnimationPlayer>("AnimationPlayer"),
                AllAnimations);
            if (AnimationPlayer != null && AnimationNameOnStart != null && StartAnimationOnReady)
            {
                AnimationPlayer.Play(AnimationNameOnStart);
            }
        }

        if (DisableOneWayCollission)
        {
            GetNode<CollisionShape2D>("CollisionShape2D").OneWayCollision = false;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
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