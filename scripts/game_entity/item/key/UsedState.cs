using System;
using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class UsedState : State<Key, Key.State>
{
    public UsedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        var transform = Entity.Transform;
        Entity.EnableCollision(Entity.KeyCollision);
        Entity.SetRigidBodyEnabled(true);
        PhysicsServer2D.BodySetState(
            Entity.GetRid(),
            PhysicsServer2D.BodyState.Transform,
            transform
        );

        Entity.LinearVelocity = Vector2.Zero;
    }
}