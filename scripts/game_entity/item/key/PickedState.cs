using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class PickedState : State<Key, Key.State>
{

    public PickedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        Entity.DisableCollision();
        Entity.SetRigidBodyEnabled(false);
    }
}