namespace project768.scripts.game_entity.npc.timeless_enemy_boss.state;

public class IdleState : BaseTimelessEnemyBossState
{
    public IdleState(TimelessEnemyBoss entity, TimelessEnemyBoss.State stateEnum) : base(entity, stateEnum)
    {
    }


    public override void PhysicProcess(double delta)
    {
        if (Entity.VisionTarget.IsAnyColliding())
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.Triggered);
        }
        
        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        Entity.MoveAndSlide();
    }
}