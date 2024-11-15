using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.jumping_enemy.state;

public class BaseJumpingEnemyState : State<JumpingEnemy, JumpingEnemy.State>
{
    public BaseJumpingEnemyState(JumpingEnemy entity, JumpingEnemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public bool TargetInVision()
    {
        bool isColliding = Entity.VisionTarget.IsColliding();
        if (isColliding && Entity.VisionTarget.GetCollider() is Player player)
        {
            Entity.TriggerPoint = player;
            return true;
        }

        return false;
    }

    public bool WillJumpOnGround()
    {
        return Entity.VisionGround.IsColliding();
    }

    public void UpdateVisionByDirection()
    {
        if (Entity.Direction.X > 0)
        {
            Entity.VisionTarget.TargetPosition = Entity.VisionTarget.TargetPosition with
            {
                X = Mathf.Abs(Entity.VisionTarget.TargetPosition.X)
            };
            Entity.VisionGround.TargetPosition = Entity.VisionGround.TargetPosition with
            {
                X = Mathf.Abs(Entity.VisionGround.TargetPosition.X)
            };
        }
        else
        {
            Entity.VisionTarget.TargetPosition = Entity.VisionTarget.TargetPosition with
            {
                X = Mathf.Abs(Entity.VisionTarget.TargetPosition.X) * -1
            };
            Entity.VisionGround.TargetPosition = Entity.VisionGround.TargetPosition with
            {
                X = Mathf.Abs(Entity.VisionGround.TargetPosition.X) * -1
            };
        }
    }

    public void CommonBodyEntered(CollisionBody body)
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
            if (body.Body is Player p && p.CurrentState.StateEnum != Player.State.Death)
            {
                p.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.FallOnEnemyHead));
                Entity.Interactor.Interact(new JumpingEnemyInteractionEvent(JumpingEnemyInteraction.KillEnemy));
            }
        }
    }
}