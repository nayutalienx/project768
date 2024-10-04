using System;
using Godot;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class RewindState : State<Key, Key.State>
{
    private Tuple<uint, uint> origCollission;

    public RewindState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        //origCollission = DisableCollision(Entity);
        Entity.SetDeferred(nameof(Entity.Freeze), true);
    }

    public override void ExitState(Key.State prevState)
    {
        Entity.SetLinearVelocity(Vector2.Zero);
        Entity.SetAngularVelocity(0.0f);
        //EnableCollision(Entity, origCollission);
        Entity.SetDeferred(nameof(Entity.Freeze), false);
    }
}