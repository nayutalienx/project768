using Godot;

namespace project768.scripts.rewind.entity;

public struct KeyRewindData
{
    public Transform2D Transform { get; set; }
    public Key.State CurrentState { get; set; }
    public ulong PickerInstanceId { get; set; }

    public KeyRewindData(Key key)
    {
        Transform = key.Transform;
        CurrentState = key.CurrentState.StateEnum;
        PickerInstanceId = key.PickerInstanceId;
    }

    public void ApplyData(Key key)
    {
        key.Transform = Transform;
        key.RewindState = (int) CurrentState;
        key.PickerInstanceId = PickerInstanceId;
    }
}