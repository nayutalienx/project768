using System;
using Godot;
using project768.scripts.common;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class MoveState : BaseEnemyState
{
    private TimerManager invertDirectionTimer = new TimerManager(0.1);

    public MoveState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        RecoverKeyOnEnterState(prevState);
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
                Entity.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.TryPickupKey)
                {
                    Key = key
                });
            }

            if (body.Body is TimelessKey timelessKey)
            {
                Entity.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.TryPickupTimelessKey)
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
                Entity.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
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


    protected void RecoverKeyOnEnterState(Enemy.State prevState)
    {
        if (prevState == Enemy.State.Rewind && Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key =
                GodotObject.InstanceFromId(Entity.InteractionContext.KeyContext.KeyInstanceId) as Key;
        }
    }

    protected void ProcessKey()
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.GlobalPosition = Entity.GlobalPosition;
        }
    }
}