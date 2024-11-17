namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class WaitState : BaseJumpingEnemyState
{
    public WaitState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {
        Entity.Visible = false;
        Entity.GlobalPosition = Entity.InitialPosition;
    }
}