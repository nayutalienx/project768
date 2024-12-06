using Godot;
using project768.scripts.game_entity.landscape.spacetime_door;

namespace project768.scripts.rewind.entity;

public struct SpacetimeLockedDoorRewindData
{
    public SpacetimeLockedDoor.State CurrentState { get; set; }
    public AnimationPlayerRewindData AnimationPlayerRewindData { get; set; }
    public Vector2 PlayerPositionWhenDoorUnlocked { get; set; }

    public SpacetimeLockedDoorRewindData(SpacetimeLockedDoor lockedDoor)
    {
        CurrentState = lockedDoor.CurrentState.StateEnum;
        AnimationPlayerRewindData = new AnimationPlayerRewindData(lockedDoor.AnimationPlayer);
        PlayerPositionWhenDoorUnlocked = lockedDoor.PlayerPositionWhenDoorUnlocked;
    }

    public void ApplyData(SpacetimeLockedDoor lockedDoor)
    {
        lockedDoor.RewindState = (int) CurrentState;
        lockedDoor.PlayerPositionWhenDoorUnlocked = PlayerPositionWhenDoorUnlocked;
        AnimationPlayerRewindData.ApplyData(lockedDoor.AnimationPlayer);
    }
}