using Godot;

namespace project768.scripts.rewind.entity;

public struct SpacetimeSwitcherRewindData
{
    public SpacetimeSwitcher.State CurrentState { get; set; }
    public float StickSpriteRotation { get; set; }
    public SpacetimeAnimationPlayerRewindData AnimationPlayer { get; set; }
    public Vector2 PlayerPositionWhenUsed { get; set; }

    public SpacetimeSwitcherRewindData(SpacetimeSwitcher switcher)
    {
        CurrentState = switcher.CurrentState.StateEnum;
        StickSpriteRotation = switcher.StickSprite.Rotation;
        AnimationPlayer = new SpacetimeAnimationPlayerRewindData(switcher.ReactAnimationPlayer);
        PlayerPositionWhenUsed = switcher.PlayerPositionWhenUsed;
    }

    public void ApplyData(SpacetimeSwitcher switcher)
    {
        switcher.RewindState = (int) CurrentState;
        switcher.StickSprite.Rotation = StickSpriteRotation;
        switcher.PlayerPositionWhenUsed = PlayerPositionWhenUsed;
        AnimationPlayer.ApplyData(switcher.ReactAnimationPlayer);
    }
}