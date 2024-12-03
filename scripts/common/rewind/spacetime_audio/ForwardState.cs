using project768.scripts.player;
using project768.scripts.state_machine;

namespace project768.scripts.rewind.spacetime_audio;

public class ForwardState : BaseSpacetimeAudioPlayerState
{
    public ForwardState(SpacetimeAudioPlayer entity, SpacetimeAudioPlayer.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(SpacetimeAudioPlayer.State prevState)
    {
        Entity.BackwardPlayer.SetStreamPaused(true);
        Entity.ForwardPlayer.SetStreamPaused(false);
        if (prevState != SpacetimeAudioPlayer.State.Forward)
        {
            Entity.SyncForwardFromBackward();
        }
    }

    public override void Process(double delta)
    {
        if (Player.Instance.PosDelta == 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Stopped);
            return;
        }
        
        if (Player.Instance.PosDelta < 0)
        {
            Entity.StateChanger.ChangeState(SpacetimeAudioPlayer.State.Backward);
            return;
        }
        
        UpdatePitchScaleFromPositionDelta();
    }
}