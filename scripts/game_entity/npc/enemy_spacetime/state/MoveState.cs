using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy_spacetime.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.rewind;

namespace project768.scripts.game_entity.npc.enemy_spacetime.state;

public class MoveState : BaseEnemySpacetimeState
{
    public MoveState(EnemySpacetime entity, EnemySpacetime.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(EnemySpacetime.State prevState)
    {
        Entity.Visible = true;
        RecoverKeyOnEnterState(prevState);
        Entity.EnableCollision(Entity.OriginalEntityLayerMask);
        Entity.HeadArea.EnableCollision(Entity.OriginalHeadAreaLayerMask);
        Entity.AttackArea.EnableCollision(Entity.OriginalAttackAreaLayerMask);
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.AreaName.Equals("attack"))
        {
            if (body.Body is Player p)
            {
                var killDir = Entity.GlobalPosition.DirectionTo(p.GlobalPosition);
                var attackAngleDeg = Mathf.RadToDeg(killDir.Angle());
                if (attackAngleDeg > 45 && attackAngleDeg < 135)
                {
                    // todo: processs enemy jump
                }

                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
            }

            if (body.Body is Key key)
            {
                Entity.Interactor.Interact(new EnemySpacetimeInteractionEvent(EnemySpacetimeInteraction.TryPickupKey)
                {
                    Key = key
                });
            }

            if (body.Body is TimelessKey timelessKey)
            {
                Entity.Interactor.Interact(
                    new EnemySpacetimeInteractionEvent(EnemySpacetimeInteraction.TryPickupTimelessKey)
                    {
                        TimelessKey = timelessKey
                    });
            }
        }

        if (body.AreaName.Equals("head"))
        {
            if (body.Body is Player p && p.CurrentState.StateEnum != Player.State.Death)
            {
                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.FallOnEnemyHead));
                Entity.Interactor.Interact(new EnemySpacetimeInteractionEvent(EnemySpacetimeInteraction.KillEnemy));
            }

            if (body.Body is Box box)
            {
                if (Mathf.Abs(box.LinearVelocity.Y) > 10)
                {
                    Entity.Interactor.Interact(new EnemySpacetimeInteractionEvent(EnemySpacetimeInteraction.KillEnemy));
                }
            }
        }
    }

    public override void PhysicProcess(double delta)
    {
        ProcessKey();
        ProcessTimelessKey();
        ProcessTimelinePosition();
        Entity.MoveAndSlide();

        if (!Entity.AliveOnStart && 
            SpacetimeRewindPlayer.Instance.GetCurrentTimelinePosition() < Entity.SpawnTimeInPixel)
        {
            Entity.LockSpawn = false;
            Entity.LockWaitingToDeath = true;
            Entity.StateChanger.ChangeState(EnemySpacetime.State.Wait);
            return;
        }
    }

    private void ProcessTimelinePosition()
    {
        Entity.GlobalPosition = Entity.SpacetimePathFollow.GlobalPosition;
    }

    protected void RecoverKeyOnEnterState(EnemySpacetime.State prevState)
    {
        if (prevState == EnemySpacetime.State.Rewind && Entity.InteractionContext.KeyContext.HasKey)
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