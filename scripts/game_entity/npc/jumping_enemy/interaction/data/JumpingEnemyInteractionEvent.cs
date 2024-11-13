using project768.scripts.common.interaction;

namespace project768.scripts.game_entity.npc.jumping_enemy.interaction.data;

public class JumpingEnemyInteractionEvent : InteractionEvent<JumpingEnemyInteraction>
{
    public JumpingEnemyInteractionEvent(JumpingEnemyInteraction interactionEnum) : base(interactionEnum)
    {
    }
}