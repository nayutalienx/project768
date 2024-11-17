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

    public bool TargetInVisionAndReachable()
    {
        bool isColliding = Entity.VisionTarget.IsAnyColliding();
        if (isColliding && Entity.VisionTarget.GetAnyCollider() is Player player)
        {
            if (IsTargetReachable(player.GlobalPosition))
            {
                Entity.TriggerPoint = player;
                return true;
            }
        }

        return false;
    }

    public bool IsTargetReachable(Vector2 target)
    {
        Entity.NavigationAgent2D.SetTargetPosition(target);
        return Entity.NavigationAgent2D.IsTargetReachable();
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