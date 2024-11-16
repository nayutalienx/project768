using Godot;
using project768.scripts.common;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class IdleState : BaseJumpingEnemyState
{
    public int JumpedInDirection;

    public IdleState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {

        if (prevState == JumpingEnemy.State.Triggered)
        {
            JumpedInDirection = 0;
        }

        Entity.EnableCollision(Entity.OriginalEntityLayerMask);
        Entity.HeadArea.EnableCollision(Entity.OriginalHeadAreaLayerMask);
        Entity.AttackArea.EnableCollision(Entity.OriginalAttackAreaLayerMask);
        Entity.Visible = true;
    }

    public override void PhysicProcess(double delta)
    {
        if (TargetInVisionAndReachable())
        {
            Entity.StateChanger.ChangeState(JumpingEnemy.State.Triggered);
            return;
        }

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }
        else
        {
            if (Entity.IdleFloorTimerManager.IsExpired())
            {
                if (
                    JumpedInDirection > Entity.JumpsToRevertDirection ||
                    !WillJumpOnGround()
                )
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

    public override void OnBodyEntered(CollisionBody body)
    {
        CommonBodyEntered(body);
    }
}