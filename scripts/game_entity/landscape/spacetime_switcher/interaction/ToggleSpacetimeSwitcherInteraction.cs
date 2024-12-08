using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.spacetime_switcher.interaction.data;

namespace project768.scripts.game_entity.landscape.spacetime_switcher.interaction;

public class ToggleSpacetimeSwitcherInteraction : Interaction<SpacetimeSwitcher, SpacetimeSwitcherInteractionEvent, SpacetimeSwitcherInteraction>
{
    public ToggleSpacetimeSwitcherInteraction(SpacetimeSwitcher entity) : base(entity)
    {
    }
    
    public override void Interact(SpacetimeSwitcherInteractionEvent eventContext)
    {
        
        if (Entity.Type == SpacetimeSwitcher.SwitcherType.Single)
        {
            Entity.StateChanger.ChangeState(SpacetimeSwitcher.State.Used);
        }
        else
        {
            if (Entity.CurrentState.StateEnum == SpacetimeSwitcher.State.Initial)
            {
                Entity.StateChanger.ChangeState(SpacetimeSwitcher.State.Used);
            }
            else
            {
                Entity.StateChanger.ChangeState(SpacetimeSwitcher.State.Initial);
            }
        }
    }
}