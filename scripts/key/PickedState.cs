using Godot;
using project768.scripts.item;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class PickedState : State<Key, Key.State>
{
    private uint originalCollisionLayer;
    private uint originalCollisionMask;

    public PickedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        originalCollisionLayer = Entity.CollisionLayer;
        originalCollisionMask = Entity.CollisionMask;
        Entity.CollisionLayer = 0;
        Entity.CollisionMask = 0;
        Entity.SetDeferred(nameof(Entity.Freeze), true);
    }

    public override void ExitState(Key.State prevState)
    {
        Entity.CollisionLayer = originalCollisionLayer;
        Entity.CollisionMask = originalCollisionMask;
        Entity.SetDeferred(nameof(Entity.Freeze), false);
    }

    public override void PhysicProcess(double delta)
    {
        Entity.Position = Entity.Picker.Position;
        if (Entity.Picker.DoorKeyPickerContext.DoorKeyEvent == DoorKeyEvent.Dropped)
        {
            Entity.StateChanger.ChangeState(Key.State.Unpicked);
        }
    }
}