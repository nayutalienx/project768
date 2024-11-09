using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.enemy.interaction.data;

public class EnemyInteractionContext : InteractionContext
{
    public EnemyKeyContext KeyContext { get; set; } = new();
    public EnemyTimelessKeyContext TimelessKeyContext { get; set; } = new();
}