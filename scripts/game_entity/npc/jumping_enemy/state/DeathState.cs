using project768.scripts.common;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class DeathState : BaseJumpingEnemyState
{
    public DeathState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {
        if (prevState != JumpingEnemy.State.Rewind)
        {
            Entity.DeathStopTimer.Reset();
        }
        
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.DeathStopTimer.IsExpired())
        {
            Entity.GlobalPosition = Entity.InitialPosition;
            Entity.Visible = false;    
        }
        else
        {
            Entity.DeathStopTimer.Update(delta);
            Entity.Velocity += Entity.GetGravity() * (float) delta;
            Entity.MoveAndSlide();
        }
    }
}