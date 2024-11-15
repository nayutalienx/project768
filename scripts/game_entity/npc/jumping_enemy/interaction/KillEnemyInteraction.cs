using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;

namespace project768.scripts.game_entity.npc.jumping_enemy.interaction;

public class KillEnemyInteraction : Interaction<JumpingEnemy, JumpingEnemyInteractionEvent, JumpingEnemyInteraction>
{
    public KillEnemyInteraction(JumpingEnemy entity) : base(entity)
    {
    }

    public override void Interact(JumpingEnemyInteractionEvent eventContext)
    {
        if (Entity.CurrentState.StateEnum == JumpingEnemy.State.Idle ||
            Entity.CurrentState.StateEnum == JumpingEnemy.State.Triggered)
        {
            Entity.StateChanger.ChangeState(JumpingEnemy.State.Death);
        }
    }
}