using project768.scripts.state_machine;

namespace project768.scripts.rewind;

public class ForwardState : State<RewindAudioPlayer, RewindAudioPlayer.State>
{
    public ForwardState(RewindAudioPlayer entity, RewindAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(RewindAudioPlayer.State prevState)
    {
        Entity.BackwardPlayer.SetStreamPaused(true);
        Entity.ForwardPlayer.SetStreamPaused(false);
        if (prevState != RewindAudioPlayer.State.Forward)
        {
            Entity.SyncForwardFromBackward();
        }
    }
}