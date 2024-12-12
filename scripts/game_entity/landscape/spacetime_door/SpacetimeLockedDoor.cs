using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_door;

public partial class SpacetimeLockedDoor :
    AnimatableBody2D,
    IStateMachineEntity<SpacetimeLockedDoor, SpacetimeLockedDoor.State>
{
    public enum State
    {
        Locked,
        Unlocked
    }

    [Export] public bool TrackEnemies { get; set; }
    [Export] public EnemySpacetime[] Enemies { get; set; }

    public int RewindState { get; set; }
    public State<SpacetimeLockedDoor, State> CurrentState { get; set; }
    public Dictionary<State, State<SpacetimeLockedDoor, State>> States { get; set; }
    public StateChanger<SpacetimeLockedDoor, State> StateChanger { get; set; }
    public AnimationPlayer AnimationPlayer { get; set; }
    public CollisionShape2D CollisionShape2D { get; set; }
    public Area2D LockArea { get; set; }
    public Label DoorLabel { get; set; }

    public float OpenDoorTimelineLength = 200.0f;
    public Vector2 PlayerPositionWhenDoorUnlocked { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        LockArea = GetNode<Area2D>("lock_area");
        LockArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("door", body)); };
        DoorLabel = GetNode<Label>("Label");

        States = new Dictionary<State, State<SpacetimeLockedDoor, State>>()
        {
            {State.Locked, new LockedState(this, State.Locked)},
            {State.Unlocked, new UnlockedState(this, State.Unlocked)},
        };
        StateChanger = new StateChanger<SpacetimeLockedDoor, State>(this);
        StateChanger.ChangeState(State.Locked);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CurrentState.Process(delta);
        if (AnimationPlayer.IsPlaying())
        {
            DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                             $"animation: {AnimationPlayer.CurrentAnimation}\n" +
                             $"pos: {AnimationPlayer.GetCurrentAnimationPosition()}";
        }
        else
        {
            if (TrackEnemies)
            {
                int aliveCounter = 0;
                foreach (EnemySpacetime enemy in Enemies)
                {
                    if (enemy.CurrentState.StateEnum != EnemySpacetime.State.Death)
                    {
                        aliveCounter++;
                    }
                }

                DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                                 $"enemies alive: {aliveCounter}\n" +
                                 $"animation: {AnimationPlayer.CurrentAnimation}";
            }
            else
            {
                DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                                 $"animation: {AnimationPlayer.CurrentAnimation}";
            }
        }
    }
}