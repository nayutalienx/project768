using System;
using Godot;
using project768.scripts.common;
using project768.scripts.key;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using RewindState = project768.scripts.key.RewindState;

public partial class Key :
    RigidBody2D,
    IRewindable,
    IStateMachineEntity<Key, Key.State>
{
    public enum State
    {
        Unpicked,
        Picked,
        Used,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Key, State> CurrentState { get; set; }
    public State<Key, State>[] States { get; set; }
    public StateChanger<Key, State> StateChanger { get; set; }
    
    public Tuple<uint, uint> KeyCollision;

    public override void _Ready()
    {
        States = new State<Key, State>[]
        {
            new UnpickedState(this, State.Unpicked),
            new PickedState(this, State.Picked),
            new UsedState(this, State.Used),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Key, State>(this);
        
        KeyCollision = this.GetCollisionLayerMask();
        
        StateChanger.ChangeState(State.Unpicked);
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
    }
}