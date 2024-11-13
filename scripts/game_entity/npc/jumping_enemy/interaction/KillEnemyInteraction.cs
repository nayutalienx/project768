using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;

namespace project768.scripts.game_entity.npc.jumping_enemy.interaction;

public class KillEnemyInteraction : Interaction<JumpingEnemy, JumpingEnemyInteractionEvent, JumpingEnemyInteraction>
{
    public KillEnemyInteraction(JumpingEnemy entity) : base(entity)
    {
    }
}