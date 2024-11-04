using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.landscape.switcher.interaction;

public class SwitcherInteractionEvent : InteractionEvent<SwitcherInteraction>
{
    public SwitcherInteractionEvent(SwitcherInteraction interactionEnum) : base(interactionEnum)
    {
    }
}