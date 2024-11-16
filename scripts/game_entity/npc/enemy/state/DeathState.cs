using project768.scripts.common;

namespace project768.scripts.enemy;

public class DeathState : BaseEnemyState
{
    public DeathState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        if (prevState != Enemy.State.Rewind)
        {
            Entity.DeathStopTimer.Reset();
        }
        
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        DropKey();
        DropTimelessKey();
    }

    public override void PhysicProcess(double delta)
    {
        
        if (Entity.DeathStopTimer.IsExpired())
        {
            Entity.StateChanger.ChangeState(Enemy.State.Wait);
        }
        else
        {
            Entity.DeathStopTimer.Update(delta);
            Entity.Velocity += Entity.GetGravity() * (float) delta;
            Entity.MoveAndSlide();
        }
        
    }
}