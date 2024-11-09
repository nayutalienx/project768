using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.game_entity.item.timeless_key;
using project768.scripts.state_machine;

public partial class TimelessKey : RigidBody2D, IStateMachineEntity<TimelessKey, TimelessKey.State>
{
    public enum State
    {
        Unpicked,
        Picked,
        Used,
    }

    public State<TimelessKey, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessKey, State>> States { get; set; }
    public StateChanger<TimelessKey, State> StateChanger { get; set; }
    public Tuple<uint, uint> KeyCollision;

    public override void _Ready()
    {
        States = new Dictionary<State, State<TimelessKey, State>>()
        {
            {State.Unpicked, new UnpickedState(this, State.Unpicked)},
            {State.Picked, new PickedState(this, State.Picked)},
            {State.Used, new UsedState(this, State.Used)},
        };
        StateChanger = new StateChanger<TimelessKey, State>(this);

        KeyCollision = this.GetCollisionLayerMask();

        StateChanger.ChangeState(State.Unpicked);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }
}