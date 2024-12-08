using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_switcher;

public class InitialState : State<SpacetimeSwitcher, SpacetimeSwitcher.State>
{
    public InitialState(SpacetimeSwitcher entity, SpacetimeSwitcher.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(SpacetimeSwitcher.State prevState)
    {
        Entity.StickSprite.Rotation = 0.0f;
        if (Entity.Reaction == SpacetimeSwitcher.SwitcherReaction.ControlAnimation)
        {
            Entity.ReactAnimationPlayer.ShouldPlay = false;                
        }
    }
}