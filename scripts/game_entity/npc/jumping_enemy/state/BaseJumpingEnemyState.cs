using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class BaseJumpingEnemyState : State<JumpingEnemy, JumpingEnemy.State>
{
    public BaseJumpingEnemyState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }
}