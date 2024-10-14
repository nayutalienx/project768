using project768.scripts.state_machine;

namespace project768.scripts.rewind;

public class BackwardState : State<RewindAudioPlayer, RewindAudioPlayer.State>
{
    public BackwardState(RewindAudioPlayer entity, RewindAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(RewindAudioPlayer.State prevState)
    {
        Entity.ForwardPlayer.SetStreamPaused(true);
        Entity.BackwardPlayer.SetStreamPaused(false);
        if (prevState != RewindAudioPlayer.State.Backward)
        {
            Entity.SyncBackwardFromForward();
        }
    }
}