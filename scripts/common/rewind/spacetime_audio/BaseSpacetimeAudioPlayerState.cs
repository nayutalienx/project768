using System;
using project768.scripts.player;
using project768.scripts.state_machine;

namespace project768.scripts.rewind.spacetime_audio;

public class BaseSpacetimeAudioPlayerState : State<SpacetimeAudioPlayer, SpacetimeAudioPlayer.State>
{
    public BaseSpacetimeAudioPlayerState(SpacetimeAudioPlayer entity, SpacetimeAudioPlayer.State stateEnum) : base(
        entity, stateEnum)
    {
    }

    public void UpdatePitchScaleFromPositionDelta()
    {
        if (Entity.PlayerPosDelta == 0)
        {
            return;
        }

        float speed = Entity.PlayerPosDelta / Player.PositionDeltaFactor;
        Entity.ForwardPlayer.PitchScale = Math.Abs(speed);
        Entity.BackwardPlayer.PitchScale = Math.Abs(speed);
    }
}