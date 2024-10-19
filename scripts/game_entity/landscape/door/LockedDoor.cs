using Godot;
using System;
using project768.scripts.common;
using project768.scripts.door;
using project768.scripts.player.interaction;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

public partial class LockedDoor :
    AnimatableBody2D,
    IRewindable,
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
    public RewindableAnimationPlayer AnimationPlayer { get; set; }
    public CollisionShape2D CollisionShape2D { get; set; }
    public Area2D LockArea { get; set; }
    public Label DoorLabel { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AnimationPlayer = new RewindableAnimationPlayer(
            GetNode<AnimationPlayer>("AnimationPlayer") as AnimationPlayer,
            new[]
            {
                "DoorOpen",
                "end",
            }
        );

        CollisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
        LockArea = GetNode<Area2D>("lock_area");
        LockArea.BodyEntered += OnBodyEntered;
        DoorLabel = GetNode<Label>("Label");

        States = new State<LockedDoor, State>[]
        {
            new LockedState(this, State.Locked),
            new UnlockedState(this, State.Unlocked),
            new RewindState(this, State.Rewind)
        };
        StateChanger = new StateChanger<LockedDoor, State>(this);
        StateChanger.ChangeState(State.Locked);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is project768.scripts.player.Player player &&
            player.InteractionContext.HasKey)
        {
            player.Interactor.Interact(
                new PlayerInteractionEvent(PlayerInteraction.UnlockedDoor)
            );
            StateChanger.ChangeState(State.Unlocked);
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CurrentState.PhysicProcess(delta);
        if (AnimationPlayer.AnimationPlayer.IsPlaying())
        {
            DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                             $"animation: {AnimationPlayer.CurrentAnimation}\n" +
                             $"pos: {AnimationPlayer.AnimationPlayer.GetCurrentAnimationPosition()}";
        }
        else
        {
            DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                             $"animation: {AnimationPlayer.CurrentAnimation}";
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
        AnimationPlayer.UpdateRewindSpeed(speed);
    }
}