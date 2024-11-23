using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.timeless_enemy_boss.interaction.data;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.interaction;

public class KillEnemyInteraction : Interaction<TimelessEnemyBoss, TimelessEnemyBossInteractionEvent,
    TimelessEnemyBossInteraction>
{
    public KillEnemyInteraction(TimelessEnemyBoss entity) : base(entity)
    {
    }

    public override void Interact(TimelessEnemyBossInteractionEvent eventContext)
    {
        if (Entity.CurrentState.StateEnum == TimelessEnemyBoss.State.Idle)
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.Triggered);
        }

        if (Entity.LifeCount > 0)
        {
            Entity.LifeCount--;
        }

        if (Entity.LifeCount == 0)
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.Death);
        }
    }
}