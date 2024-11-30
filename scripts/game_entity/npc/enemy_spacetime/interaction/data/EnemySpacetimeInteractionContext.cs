using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;

public class EnemySpacetimeInteractionContext : InteractionContext
{
    public EnemySpacetimeKeyContext KeyContext { get; set; } = new();
    public EnemySpacetimeTimelessKeyContext TimelessKeyContext { get; set; } = new();    
}