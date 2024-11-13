using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.timeless_enemy.interaction.data;

public class TimelessEnemyInteractionContext : InteractionContext
{
    public TimelessEnemyKeyContext KeyContext { get; set; } = new();
    public TimelessEnemyTimelessKeyContext TimelessKeyContext { get; set; } = new();
}