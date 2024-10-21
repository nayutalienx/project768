using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public class MoveState : State<CannonBall, CannonBall.State>
{
    public MoveState(CannonBall entity, CannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(CannonBall.State prevState)
    {
        Entity.ShowBall();
    }

    public override void PhysicProcess(double delta)
    {
        Vector2 moveDirection = Vector2.Right.Rotated(Entity.Transform.Rotation).Normalized();
        Entity.Transform = Entity.Transform.Translated(moveDirection * (float) (Entity.Speed * delta));
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Enemy enemy)
        {
            enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
        }

        Entity.StateChanger.ChangeState(CannonBall.State.Wait);
    }
}