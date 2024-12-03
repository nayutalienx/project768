using project768.scripts.player;
using project768.scripts.state_machine;

namespace project768.scripts.rewind.spacetime_audio;

public class BackwardState : BaseSpacetimeAudioPlayerState
{
    public BackwardState(SpacetimeAudioPlayer entity, SpacetimeAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(SpacetimeAudioPlayer.State prevState)
    {
        Entity.ForwardPlayer.SetStreamPaused(true);
        Entity.BackwardPlayer.SetStreamPaused(false);
        if (prevState != SpacetimeAudioPlayer.State.Backward)
        {
            Entity.SyncBackwardFromForward();
        }
    }

    public override void Process(double delta)
    {
        if (Player.Instance.PosDelta == 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Stopped);
            return;
        }
        
        if (Player.Instance.PosDelta > 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Forward);
            return;
        }
        
        UpdatePitchScaleFromPositionDelta();
    }
}