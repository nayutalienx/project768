using Godot;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class IdleState : BaseJumpingEnemyState
{
    public int JumpedInDirection;

    public IdleState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }
        else
        {
            if (Entity.IdleFloorTimerManager.IsExpired())
            {
                if (JumpedInDirection > Entity.JumpsToRevertDirection)
                {
                    Entity.Direction = Entity.Direction.Reflect(Vector2.Up);
                    JumpedInDirection = 0;
                }

                JumpedInDirection++;
                Entity.Velocity = Entity.Direction;
                Entity.IdleFloorTimerManager.Reset();
            }
            else
            {
                Entity.Velocity = Entity.Velocity.MoveToward(
                    Entity.Velocity with
                    {
                        X = 0
                    }, Entity.Friction
                );
                Entity.IdleFloorTimerManager.Update(delta);
            }
        }

        Entity.MoveAndSlide();
    }
}