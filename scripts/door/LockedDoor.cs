using Godot;
using System;
using project768.scripts.door;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class LockedDoor :
    AnimatableBody2D,
    Rewindable,
    IStateMachineEntity<LockedDoor, LockedDoor.State>
{
    public enum State
    {
        Locked,
        Unlocked,
        Rewind
    }

    public int RewindState { get; set; }
    public State<LockedDoor, State> CurrentState { get; set; }
    public State<LockedDoor, State>[] States { get; set; }
    public StateChanger<LockedDoor, State> StateChanger { get; set; }

    public AnimationPlayer AnimationPlayer { get; set; }

    private CollisionShape2D collisionShape2D;
    private Area2D lockArea;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        States = new State<LockedDoor, State>[]
        {
            new LockedState(this, State.Locked),
            new UnlockedState(this, State.Unlocked),
            new RewindState(this, State.Rewind)
        };
        StateChanger = new StateChanger<LockedDoor, State>(this);
        StateChanger.ChangeState(State.Locked);

        AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        lockArea = GetNode<Area2D>("lock_area");

        lockArea.BodyEntered += OnBodyEntered;

        //animationPlayer.AnimationFinished += name => collisionShape2D.Disabled = true;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is project768.scripts.player.Player player &&
            player.DoorKeyPickerContext.HasKey)
        {
            StateChanger.ChangeState(State.Unlocked);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
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
}