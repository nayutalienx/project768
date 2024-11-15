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