using project768.scripts.common.interaction;
using project768.scripts.game_entity.landscape.switcher.interaction;

namespace project768.scripts.game_entity.landscape.timeless_switcher.interaction.data;

public class TimelessSwitcherInteractionEvent : InteractionEvent<TimelessSwitcherInteraction>
{
    public TimelessSwitcherInteractionEvent(TimelessSwitcherInteraction interactionEnum) : base(interactionEnum)
    {
    }
}