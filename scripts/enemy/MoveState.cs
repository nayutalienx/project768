using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class MoveState : State<Enemy, Enemy.State>
{
    private TimerManager invertDirectionTimer = new TimerManager(0.1);

    public MoveState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        Entity.EnableCollision(Entity.OriginalEntityLayerMask);
        Entity.HeadArea.EnableCollision(Entity.OriginalHeadAreaLayerMask);
        Entity.AttackArea.EnableCollision(Entity.OriginalAttackAreaLayerMask);
    }

    public override void PhysicProcess(double delta)
    {
        bool invertDirectionTimerFinished = invertDirectionTimer.Update(delta);

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        if (
            !Entity.FallRaycastLeft.IsColliding() ||
            !Entity.FallRaycastRight.IsColliding()
            || Entity.IsOnWall()
        )
        {
            if (Entity.IsOnFloor() && invertDirectionTimerFinished)
            {
                invertDirectionTimer.Reset();
                Entity.EnemyDirection *= -1;
            }
        }

        Entity.Velocity = Entity.Velocity with {X = Entity.EnemyDirection * Entity.MoveSpeed};
        Entity.MoveAndSlide();
    }
}