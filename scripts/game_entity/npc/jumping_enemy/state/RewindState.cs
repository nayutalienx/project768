using project768.scripts.common;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class RewindState : BaseJumpingEnemyState
{
    public RewindState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {
        Entity.DisableCollision();
    }
}