using Godot;
using project768.scripts.player;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.state;

public class TriggeredToTargetState : BaseTimelessEnemyBossState
{
    public TriggeredToTargetState(TimelessEnemyBoss entity, TimelessEnemyBoss.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        GodotObject target = Entity.VisionTarget.GetAnyCollider();
        if (target == null)
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.Triggered);
            return;
        }

        Player player = target as Player;

        if (!IsTriggeredTargetInField(player.GlobalPosition))
        {
            Entity.StateChanger.ChangeState(TimelessEnemyBoss.State.Triggered);
            return;
        }

        Entity.TargetToMove = player.GlobalPosition;

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        UpdateVelocityMoveToTarget();
        Entity.MoveAndSlide();
    }
}