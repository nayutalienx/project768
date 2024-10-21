using Godot;

namespace project768.scripts.rewind.entity;

public struct LockedDoorRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public LockedDoor.State CurrentState { get; set; }

    public AnimationPlayerRewindData AnimationPlayerRewindData { get; set; }

    public LockedDoorRewindData(LockedDoor lockedDoor)
    {
        GlobalPosition = lockedDoor.GlobalPosition;
        CurrentState = lockedDoor.CurrentState.StateEnum;
        AnimationPlayerRewindData = new AnimationPlayerRewindData(lockedDoor.AnimationPlayer);
    }

    public void ApplyData(LockedDoor lockedDoor)
    {
        lockedDoor.GlobalPosition = GlobalPosition;
        lockedDoor.RewindState = (int) CurrentState;
        AnimationPlayerRewindData.ApplyData(lockedDoor.AnimationPlayer);
    }
}