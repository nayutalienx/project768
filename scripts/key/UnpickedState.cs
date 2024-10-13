using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class UnpickedState : State<Key, Key.State>
{
    public UnpickedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        Entity.PickerArea.EnableCollision(Entity.PickerAreaCollision);
        Entity.EnableCollision(Entity.KeyCollision);
        Entity.SetRigidBodyEnabled(true);
        
        Entity.LinearVelocity = Vector2.Zero;
    }
}