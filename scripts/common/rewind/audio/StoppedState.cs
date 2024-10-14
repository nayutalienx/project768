using project768.scripts.state_machine;

namespace project768.scripts.rewind;

public class StoppedState : State<RewindAudioPlayer, RewindAudioPlayer.State>
{
    public StoppedState(RewindAudioPlayer entity, RewindAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(RewindAudioPlayer.State prevState)
    {
        if (prevState == RewindAudioPlayer.State.Backward)
        {
            Entity.BackwardPlayer.SetStreamPaused(true);
            Entity.ForwardPlayer.SetStreamPaused(false);
            Entity.SyncForwardFromBackward();
            Entity.ForwardPlayer.SetStreamPaused(true);
        }
        else if (prevState == RewindAudioPlayer.State.Forward)
        {
            Entity.ForwardPlayer.SetStreamPaused(true);
            Entity.BackwardPlayer.SetStreamPaused(false);
            Entity.SyncBackwardFromForward();
            Entity.BackwardPlayer.SetStreamPaused(true);
        }
    }
}