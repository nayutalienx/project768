using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.interaction.data;

public class TimelessEnemyBossInteractionEvent : InteractionEvent<TimelessEnemyBossInteraction>
{
    public TimelessEnemyBossInteractionEvent(TimelessEnemyBossInteraction interactionEnum) : base(interactionEnum)
    {
    }
}