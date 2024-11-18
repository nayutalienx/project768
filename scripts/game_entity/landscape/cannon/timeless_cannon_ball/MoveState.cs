using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.jumping_enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.timeless_cannon_ball;

public class MoveState : State<TimelessCannonBall, TimelessCannonBall.State>
{
    public MoveState(TimelessCannonBall entity, TimelessCannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessCannonBall.State prevState)
    {
        Entity.ShowBall();
    }

    public override void PhysicProcess(double delta)
    {
        Entity.GlobalTransform = Entity.GlobalTransform.Translated(Entity.Direction * (float) (Entity.Speed * delta));
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Enemy enemy)
        {
            enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
        }

        if (body.Body is Player player)
        {
            player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
        }

        if (body.Body is TimelessEnemy timelessEnemy)
        {
            timelessEnemy.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
        }

        if (body.Body is JumpingEnemy jumpingEnemy)
        {
            jumpingEnemy.Interactor.Interact(new JumpingEnemyInteractionEvent(JumpingEnemyInteraction.KillEnemy));
        }

        Entity.StateChanger.ChangeState(TimelessCannonBall.State.Wait);
    }
}