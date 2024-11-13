using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;

namespace project768.scripts.game_entity.npc.timeless_enemy.state;


public class MoveState : BaseEnemyState
{
    private TimerManager invertDirectionTimer = new TimerManager(0.1);

    public MoveState(TimelessEnemy entity, TimelessEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessEnemy.State prevState)
    {
        Entity.EnableCollision(Entity.OriginalEntityLayerMask);
        Entity.HeadArea.EnableCollision(Entity.OriginalHeadAreaLayerMask);
        Entity.AttackArea.EnableCollision(Entity.OriginalAttackAreaLayerMask);
        Entity.Visible = true;
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.AreaName.Equals("attack"))
        {
            if (body.Body is Player p)
            {
                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
            }

            if (body.Body is Key key)
            {
                Entity.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.TryPickupKey)
                {
                    Key = key
                });
            }

            if (body.Body is TimelessKey timelessKey)
            {
                Entity.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.TryPickupTimelessKey)
                {
                    TimelessKey = timelessKey
                });
            }
        }

        if (body.AreaName.Equals("head"))
        {
            if (body.Body is Player p)
            {
                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.FallOnEnemyHead));
                Entity.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
            }
        }
    }

    public override void PhysicProcess(double delta)
    {
        ProcessKey();
        ProcessTimelessKey();
        bool invertDirectionTimerFinished = invertDirectionTimer.Update(delta);

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        if (Entity.IsOnWall())
        {
            if (invertDirectionTimerFinished)
            {
                invertDirectionTimer.Reset();
                Entity.EnemyDirection *= -1;
            }
        }

        Entity.Velocity = Entity.Velocity.MoveToward(
            Entity.Velocity with
            {
                X = Entity.MoveSpeed * Entity.EnemyDirection
            }, Entity.Friction
        );

        Entity.Velocity = Entity.Velocity with
        {
            X = Mathf.Clamp(Entity.Velocity.X, Entity.VelocityLimit.X * -1, Entity.VelocityLimit.X),
            Y = Mathf.Clamp(Entity.Velocity.Y, Entity.VelocityLimit.Y * -1, Entity.VelocityLimit.Y),
        };

        Entity.MoveAndSlide();
    }
    

    protected void ProcessKey()
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
}