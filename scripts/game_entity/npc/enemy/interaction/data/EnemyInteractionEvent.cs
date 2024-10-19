using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.enemy.interaction.data;

public class EnemyInteractionEvent : InteractionEvent<EnemyInteraction>
{
    public Key Key { get; set; }

    public EnemyInteractionEvent(EnemyInteraction interactionEnum) : base(interactionEnum)
    {
    }
}