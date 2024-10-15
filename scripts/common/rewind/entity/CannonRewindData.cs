namespace project768.scripts.rewind.entity;

public struct CannonRewindData
{
    public double Timer { get; set; }
    public CannonBallRewindData[] CannonBallRewindDatas { get; set; }

    public CannonRewindData(Cannon cannon)
    {
        Timer = cannon.TimerManager.CurrentTime;
        CannonBallRewindDatas =
            CommonRewindData.CreateRewindData(cannon.CannonBallPool, ball => new CannonBallRewindData(ball));
    }

    public void ApplyData(Cannon cannon)
    {
        cannon.TimerManager.CurrentTime = Timer;
        CommonRewindData.ApplyRewindData(cannon.CannonBallPool, CannonBallRewindDatas,
            (rewind, ball) => { rewind.ApplyData(ball); });
    }
}