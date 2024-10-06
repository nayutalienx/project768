using Godot;

namespace project768.scripts.rewind.entity;

public struct LockedDoorRewindData
{
    public Vector2 Position { get; set; }
    public LockedDoor.State CurrentState { get; set; }

    public LockedDoorRewindData(LockedDoor lockedDoor)
    {
        Position = lockedDoor.Position;
        CurrentState = lockedDoor.CurrentState.StateEnum;
    }

    public void ApplyState(LockedDoor lockedDoor)
    {
        lockedDoor.Position = Position;
        lockedDoor.RewindState = (int) CurrentState;
    }
}