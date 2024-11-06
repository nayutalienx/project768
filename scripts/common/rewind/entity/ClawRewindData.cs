namespace project768.scripts.rewind.entity;

public struct ClawRewindData
{
    public AnimationPlayerRewindData AnimationPlayerRewindData { get; set; }

    public ClawRewindData(Claw claw)
    {
        AnimationPlayerRewindData = new AnimationPlayerRewindData(claw.AnimationPlayer);
    }

    public void ApplyData(Claw claw)
    {
        AnimationPlayerRewindData.ApplyData(claw.AnimationPlayer);
    }
}