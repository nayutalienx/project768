using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.timeless_enemy.interaction.data;

public class TimelessEnemyInteractionEvent : InteractionEvent<TimelessEnemyInteraction>
{
    public Key Key { get; set; }
    public TimelessKey TimelessKey { get; set; }

    public TimelessEnemyInteractionEvent(TimelessEnemyInteraction interactionEnum) : base(interactionEnum)
    {
    }
}