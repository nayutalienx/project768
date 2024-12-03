using project768.scripts.player;

namespace project768.scripts.rewind.spacetime_audio;

public class StoppedState : BaseSpacetimeAudioPlayerState
{
    public StoppedState(SpacetimeAudioPlayer entity, SpacetimeAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(SpacetimeAudioPlayer.State prevState)
    {
        if (prevState == SpacetimeAudioPlayer.State.Backward)
        {
            Entity.BackwardPlayer.SetStreamPaused(true);
            Entity.ForwardPlayer.SetStreamPaused(false);
            Entity.SyncForwardFromBackward();
            Entity.ForwardPlayer.SetStreamPaused(true);
        }
        else if (prevState == SpacetimeAudioPlayer.State.Forward)
        {
            Entity.ForwardPlayer.SetStreamPaused(true);
            Entity.BackwardPlayer.SetStreamPaused(false);
            Entity.SyncBackwardFromForward();
            Entity.BackwardPlayer.SetStreamPaused(true);
        }
    }

    public override void Process(double delta)
    {
        
        if (Player.Instance.PosDelta < 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Backward);
            return;
        }
        
        if (Player.Instance.PosDelta > 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Forward);
            return;
        }
    }
}