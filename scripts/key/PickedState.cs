using System;
using Godot;
using project768.scripts.item;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class PickedState : State<Key, Key.State>
{
    private Tuple<uint, uint> pickerAreaCollission;
    private Tuple<uint, uint> rbCollission;

    public PickedState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        pickerAreaCollission = DisableCollision(Entity.PickerArea);
        rbCollission = DisableCollision(Entity);
        Entity.SetDeferred(nameof(Entity.Freeze), true);
    }

    public override void ExitState(Key.State prevState)
    {
        EnableCollision(Entity.PickerArea, pickerAreaCollission);
        EnableCollision(Entity, rbCollission);
        Entity.SetDeferred(nameof(Entity.Freeze), false);
    }

    public override void PhysicProcess(double delta)
    {
        Entity.Transform = Entity.Picker.Transform;
        if (Entity.Picker.DoorKeyPickerContext.DoorKeyEvent == DoorKeyEvent.Dropped)
        {
            Entity.StateChanger.ChangeState(Key.State.Unpicked);
        }
    }
}