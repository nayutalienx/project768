using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.enemy.interaction.data;

public class EnemyInteractionContext : InteractionContext
{
    // Key
    public bool HasKey { get; set; }
    public Key Key { get; set; }
    public ulong KeyInstanceId { get; set; }
}