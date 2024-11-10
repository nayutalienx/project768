
using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.timeless_door;
using project768.scripts.state_machine;


public partial class TimelessLockedDoor :
    AnimatableBody2D,
    IStateMachineEntity<TimelessLockedDoor, TimelessLockedDoor.State>
{
    public enum State
    {
        Locked,
        Unlocked
    }

    [Export] public bool TrackEnemies { get; set; }
    [Export] public Enemy[] Enemies { get; set; }
    
    public State<TimelessLockedDoor, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessLockedDoor, State>> States { get; set; }
    public StateChanger<TimelessLockedDoor, State> StateChanger { get; set; }
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
        LockArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("door", body)); };
        DoorLabel = GetNode<Label>("Label");

        States = new Dictionary<State, State<TimelessLockedDoor, State>>()
        {
            {State.Locked, new LockedState(this, State.Locked)},
            {State.Unlocked, new UnlockedState(this, State.Unlocked)},
        };
        StateChanger = new StateChanger<TimelessLockedDoor, State>(this);
        StateChanger.ChangeState(State.Locked);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        CurrentState.Process(delta);
        if (AnimationPlayer.AnimationPlayer.IsPlaying())
        {
            DoorLabel.Text = $"state: {CurrentState.StateEnum}\n" +
                             $"animation: {AnimationPlayer.CurrentAnimation}\n" +
                             $"pos: {AnimationPlayer.AnimationPlayer.GetCurrentAnimationPosition()}";
        }
        else
        {
            if (TrackEnemies)
            {
                int aliveCounter = 0;
                foreach (Enemy enemy in Enemies)
                {
                    if (enemy.CurrentState.StateEnum != Enemy.State.Death)
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