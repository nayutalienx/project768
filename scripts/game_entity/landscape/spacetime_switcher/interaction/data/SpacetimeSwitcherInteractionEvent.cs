using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.landscape.spacetime_switcher.interaction.data;

public class SpacetimeSwitcherInteractionEvent : InteractionEvent<SpacetimeSwitcherInteraction>
{
    public SpacetimeSwitcherInteractionEvent(SpacetimeSwitcherInteraction interactionEnum) : base(interactionEnum)
    {
    }
}