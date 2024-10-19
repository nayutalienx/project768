namespace project768.scripts.rewind.entity;

public struct SwitcherRewindData
{
    public Switcher.State CurrentState { get; set; }
    public AnimationPlayerRewindData AnimationPlayer { get; set; }

    public SwitcherRewindData(Switcher switcher)
    {
        CurrentState = switcher.CurrentState.StateEnum;
        AnimationPlayer = new AnimationPlayerRewindData(switcher.AnimationPlayer);
    }

    public void ApplyData(Switcher switcher)
    {
        switcher.RewindState = (int) CurrentState;
        AnimationPlayer.ApplyData(switcher.AnimationPlayer);
    }
}