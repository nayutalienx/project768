using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class MoveState : State<Enemy, Enemy.State>
{
    public MoveState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
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
            if (Entity.IsOnFloor())
            {
                Entity.EnemyDirection *= -1;
            }
        }

        Entity.Velocity = Entity.Velocity with {X = Entity.EnemyDirection * Entity.MoveSpeed};
        Entity.MoveAndSlide();
    }
}