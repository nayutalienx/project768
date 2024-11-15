using project768.scripts.common;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class DeathState : BaseJumpingEnemyState
{
    public DeathState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {
        Entity.GlobalPosition = Entity.InitialPosition;
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        Entity.Visible = false;
    }
}