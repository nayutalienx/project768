using System;
using Godot;
using project768.scripts.state_machine;

namespace project768.scripts.key;

public class RewindState : State<Key, Key.State>
{
    private Tuple<uint, uint> pickerAreaCollission;

    public RewindState(Key entity, Key.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Key.State prevState)
    {
        pickerAreaCollission = DisableCollision(Entity.PickerArea);
        Entity.SetDeferred(nameof(Entity.Freeze), true);
    }

    public override void ExitState(Key.State prevState)
    {
        EnableCollision(Entity.PickerArea, pickerAreaCollission);
        Entity.SetDeferred(nameof(Entity.Freeze), false);
    }
}