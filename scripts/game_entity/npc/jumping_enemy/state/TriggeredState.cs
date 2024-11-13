namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class TriggeredState : BaseJumpingEnemyState
{
    public TriggeredState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        
        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }
        
        Entity.MoveAndSlide();
    }
}