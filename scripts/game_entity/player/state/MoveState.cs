using Godot;
using project768.scripts.common;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class MoveState : BasePlayerState
{
    public MoveState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void HandleInput(InputEvent _event)
    {
        Entity.HandleInput(_event);
    }

    public override void EnterState(Player.State prevState)
    {
        RecoverKeyOnEnterState(prevState);
        Entity.EnableCollision(Entity.OrigCollission);
        if (prevState == Player.State.Rewind)
        {
            Entity.CleanCache();
        }
    }

    public override void PhysicProcess(double delta)
    {
        if ((Entity.Cache.DownPressed || Entity.Cache.UpPressed) &&
            Entity.InteractionContext.Ladder != Vector2.Zero)
        {
            Entity.StateChanger.ChangeState(Player.State.Ladder);
            return;
        }

        ProcessKey();

        if (!Entity.IsOnFloor())
        {
            Entity.Velocity += Entity.GetGravity() * (float) delta;
        }

        if (Entity.Cache.JumpPressed && Entity.IsOnFloor())
        {
            Entity.Velocity = Entity.Velocity with {Y = Entity.JumpVelocity};
        }

        float direction = Entity.Cache.HorizontalDirection;

        Entity.Velocity = Entity.Velocity with {X = direction * Entity.MoveSpeed};
        if (direction == 0)
        {
            Entity.Velocity = Entity.Velocity.MoveToward(
                Entity.Velocity with {X = 0}, Entity.MoveSpeed
            );
        }

        if (Entity.Cache.DownPressed)
        {
            Entity.Position = Entity.Position with {Y = Entity.Position.Y + 1};
        }

        Entity.MoveAndSlide();
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Key key)
        {
            Entity.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.TryPickupKey)
            {
                Key = key
            });
        }
    }
}