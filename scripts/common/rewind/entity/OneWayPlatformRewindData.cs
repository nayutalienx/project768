using Godot;

namespace project768.scripts.rewind.entity;

public struct OneWayPlatformRewindData
{
    public Vector2 GlobalPosition { get; set; }
    public AnimationPlayerRewindData AnimationPlayerRewindData { get; set; }

    public OneWayPlatformRewindData(OneWayPlatform oneWayPlatform)
    {
        GlobalPosition = oneWayPlatform.GlobalPosition;
        if (oneWayPlatform.AnimationPlayer != null)
        {
            AnimationPlayerRewindData = new AnimationPlayerRewindData(oneWayPlatform.AnimationPlayer);
        }
        else
        {
            AnimationPlayerRewindData = new AnimationPlayerRewindData();
        }
    }

    public void ApplyData(OneWayPlatform oneWayPlatform)
    {
        if (oneWayPlatform.AnimationPlayer != null)
        {
            AnimationPlayerRewindData.ApplyData(oneWayPlatform.AnimationPlayer);
        }

        oneWayPlatform.GlobalPosition = GlobalPosition;
    }
}