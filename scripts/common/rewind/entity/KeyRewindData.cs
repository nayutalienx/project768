using Godot;

namespace project768.scripts.rewind.entity;

public struct KeyRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public Key.State CurrentState { get; set; }

    public KeyRewindData(Key key)
    {
        GlobalPosition = key.GlobalPosition;
        CurrentState = key.CurrentState.StateEnum;
    }

    public void ApplyData(Key key)
    {
        key.GlobalPosition = GlobalPosition;
        key.RewindState = (int) CurrentState;
    }
}