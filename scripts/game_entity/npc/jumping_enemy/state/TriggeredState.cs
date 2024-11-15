using Godot;
using project768.scripts.common;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class TriggeredState : BaseJumpingEnemyState
{
    public TriggeredState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(JumpingEnemy.State prevState)
    {
        Entity.EnableCollision(Entity.OriginalEntityLayerMask);
        Entity.HeadArea.EnableCollision(Entity.OriginalHeadAreaLayerMask);
        Entity.AttackArea.EnableCollision(Entity.OriginalAttackAreaLayerMask);
        Entity.Visible = true;
    }

    public override void PhysicProcess(double delta)
    {
        UpdateVisionByDirection();

        if (!TargetInVision())
        {
            Entity.StateChanger.ChangeState(JumpingEnemy.State.Idle);
            return;
        }

        var isOnFloor = Entity.IsOnFloor();

        if (!isOnFloor)
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        if (isOnFloor)
        {
            UpdateDirectionToTriggerPoint();

            if (!WillJumpOnGround())
            {
                Entity.Direction = Entity.Direction.Reflect(Vector2.Up);
                Entity.StateChanger.ChangeState(JumpingEnemy.State.Idle);
                return;
            }

            if (MustJumpAttack())
            {
                Entity.Velocity = Entity.Direction * Entity.JumpAttackDirectionScale;
            }
            else
            {
                Entity.Velocity = Entity.Direction * Entity.TriggeredDirectionScale;
            }
        }

        Entity.JumpAttackTimerManager.Update(delta);

        Entity.MoveAndSlide();
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        CommonBodyEntered(body);
    }

    private bool MustJumpAttack()
    {
        if (Entity.JumpAttackTimerManager.IsExpired())
        {
            Vector2 triggerPos = Entity.TriggerPoint.GlobalPosition;
            Vector2 enemyPos = Entity.GlobalPosition;
            var mustJumpAttack = enemyPos.DistanceTo(triggerPos) < Entity.JumpAttackDistance;

            if (mustJumpAttack)
            {
                Entity.JumpAttackTimerManager.Reset();
            }

            return mustJumpAttack;
        }

        return false;
    }

    private void UpdateDirectionToTriggerPoint()
    {
        Vector2 triggerPos = Entity.TriggerPoint.GlobalPosition;
        Vector2 enemyPos = Entity.GlobalPosition;
        Vector2 directionTo = enemyPos.DirectionTo(triggerPos);
        float angleMod = Mathf.Abs(Mathf.RadToDeg(directionTo.Angle()));

        if (angleMod > 90) // LEFT SIDE
        {
            Entity.Direction = Entity.Direction with
            {
                X = Mathf.Abs(Entity.Direction.X) * -1
            };
        }
        else // RIGHT SIDE
        {
            Entity.Direction = Entity.Direction with
            {
                X = Mathf.Abs(Entity.Direction.X)
            };
        }
    }
}