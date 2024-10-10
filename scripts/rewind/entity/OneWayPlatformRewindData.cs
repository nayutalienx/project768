namespace project768.scripts.rewind.entity;

public struct OneWayPlatformRewindData
{
    public AnimationPlayerRewindData AnimationPlayerRewindData { get; set; }

    public OneWayPlatformRewindData(OneWayPlatform oneWayPlatform)
    {
        AnimationPlayerRewindData = new AnimationPlayerRewindData(oneWayPlatform.AnimationPlayer);
    }

    public void ApplyData(OneWayPlatform oneWayPlatform)
    {
        AnimationPlayerRewindData.ApplyData(oneWayPlatform.AnimationPlayer);
    }

}