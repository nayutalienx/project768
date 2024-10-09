using System;
using Godot;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class UsedState : State<Key, Key.State>
{
    public UsedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        Entity.LinearVelocity = Vector2.Zero;
    }
}