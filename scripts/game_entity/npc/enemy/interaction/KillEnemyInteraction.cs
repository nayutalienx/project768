using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;

namespace project768.scripts.game_entity.npc.enemy.interaction;

public class KillEnemyInteraction : Interaction<Enemy, EnemyInteractionEvent, EnemyInteraction>
{
    public KillEnemyInteraction(Enemy entity) : base(entity)
    {
    }

    public override void Interact(EnemyInteractionEvent eventContext)
    {
        Entity.StateChanger.ChangeState(Enemy.State.Death);
    }
}