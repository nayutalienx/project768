using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.timeless_enemy_boss.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.timeless_enemy_boss.state;

public class BaseTimelessEnemyBossState : State<TimelessEnemyBoss, TimelessEnemyBoss.State>
{
    RandomNumberGenerator RandomNumberGenerator = new();

    public BaseTimelessEnemyBossState(TimelessEnemyBoss entity, TimelessEnemyBoss.State stateEnum) : base(entity,
        stateEnum)
    {
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.AreaName.Equals("attack"))
        {
            if (body.Body is Player p)
            {
                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
            }
        }

        if (body.AreaName.Equals("head"))
        {
            if (body.Body is Box box)
            {
                if (Mathf.Abs(box.LinearVelocity.Y) > 10)
                {
                    Entity.Interactor.Interact(
                        new TimelessEnemyBossInteractionEvent(TimelessEnemyBossInteraction.KillBoss));
                }
            }
        }
    }

    public void SetRandomTargetToMove()
    {
        var lineGlobalPosition = Entity.FieldLine.GlobalPosition;
        var linePoints = Entity.FieldLine.GetPoints();
        var newTarget = (linePoints[0] + lineGlobalPosition).Lerp(
            linePoints[1] + lineGlobalPosition,
            RandomNumberGenerator.RandfRange(0, 1));
        Entity.TargetToMove = newTarget;
    }

    public bool IsTargetReached()
    {
        return IsInRange(
            Entity.GlobalPosition.X,
            Entity.TargetToMove.X - Entity.EnemyRadius,
            Entity.TargetToMove.X + Entity.EnemyRadius
        );
    }

    public bool IsTriggeredTargetInField(Vector2 target)
    {
        var linePoints = Entity.FieldLine.GetPoints();
        return IsInRange(
            target.X,
            linePoints[0].X,
            linePoints[1].X
        );
    }

    public void UpdateVelocityMoveToTarget()
    {
        Vector2 directionTo = Entity.GlobalPosition.DirectionTo(Entity.TargetToMove);
        float angleMod = Mathf.Abs(Mathf.RadToDeg(directionTo.Angle()));

        if (angleMod > 90) // LEFT SIDE
        {
            Entity.Velocity = new Vector2(Entity.MoveSpeed * -1, 0);
        }
        else // RIGHT SIDE
        {
            Entity.Velocity = new Vector2(Entity.MoveSpeed, 0);
        }
    }

    public bool IsInRange(float value, float min, float max)
    {
        return value >= min && value <= max;
    }
}