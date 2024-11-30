using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;

public class EnemySpacetimeInteractionEvent : InteractionEvent<EnemySpacetimeInteraction>
{
    
    public Key Key { get; set; }
    public TimelessKey TimelessKey { get; set; }

    public EnemySpacetimeInteractionEvent(EnemySpacetimeInteraction interactionEnum) : base(interactionEnum)
    {
    }
}