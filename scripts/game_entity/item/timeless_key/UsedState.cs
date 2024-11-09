using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.item.timeless_key;

public class UsedState : State<TimelessKey, TimelessKey.State>
{
    public UsedState(TimelessKey entity, TimelessKey.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessKey.State prevState)
    {
        Entity.EnableCollision(Entity.KeyCollision);
        Entity.SetRigidBodyEnabled(true);
        Entity.GlobalPosition = Entity.GlobalPosition;
        Entity.LinearVelocity = Vector2.Zero;
    }
}