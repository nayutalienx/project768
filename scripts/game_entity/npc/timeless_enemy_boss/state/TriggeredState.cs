using Godot;
using project768.scripts.player;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.state;

public class TriggeredState : BaseTimelessEnemyBossState
{
    public TriggeredState(TimelessEnemyBoss entity, TimelessEnemyBoss.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessEnemyBoss.State prevState)
    {
        SetRandomTargetToMove();
    }

    public override void PhysicProcess(double delta)
    {
        GodotObject godotObject = Entity.VisionTarget.GetAnyCollider();
        if (godotObject != null &&
            godotObject is Player player &&
            IsTriggeredTargetInField(player.GlobalPosition))
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.TriggeredToTarget);
            return;
        }

        if (IsTargetReached())
        {
            SetRandomTargetToMove();
            return;
        }

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        UpdateVelocityMoveToTarget();
        Entity.MoveAndSlide();
    }
}