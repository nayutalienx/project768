using Godot;

namespace project768.scripts.rewind.entity;

public struct KeyRewindData
{
    public Vector2 Position { get; set; }
    public Key.State CurrentState { get; set; }

    public KeyRewindData(Key key)
    {
        Position = key.Position;
        CurrentState = key.CurrentState.StateEnum;
    }

    public void ApplyData(Key key)
    {
        key.Position = Position;
        key.RewindState = (int) CurrentState;
    }
}