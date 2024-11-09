using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.item.timeless_key;

public class PickedState : State<TimelessKey, TimelessKey.State>
{
    public PickedState(TimelessKey entity, TimelessKey.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessKey.State prevState)
    {
        Entity.DisableCollision();
        Entity.SetRigidBodyEnabled(false);
    }
}