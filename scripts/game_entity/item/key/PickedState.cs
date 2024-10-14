using System;
using Godot;
using project768.scripts.common;
using project768.scripts.item;
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

    public override void PhysicProcess(double delta)
    {
        Entity.Transform = Entity.Picker.Transform;

        DoorKeyEvent keyEvent = Entity.Picker.DoorKeyPickerContext.ConsumeEvent();

        if (keyEvent == DoorKeyEvent.Dropped)
        {
            Entity.StateChanger.ChangeState(Key.State.Unpicked);
            return;
        }

        if (keyEvent == DoorKeyEvent.Used)
        {
            Entity.StateChanger.ChangeState(Key.State.Used);
            return;
        }
    }
}