using Godot;
using project768.scripts.common;

namespace project768.scripts.player;

public class LadderGridState : BasePlayerState
{
    public LadderGridState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        Entity.JumpMultiplier = 1.0f;
        RecoverKeyOnEnterState(prevState);
        Entity.EnableCollision(Entity.OrigCollission);
        if (prevState == Player.State.Rewind)
        {
            Entity.CleanCache();
        }
    }

    public override void HandleInput(InputEvent _event)
    {
        Entity.HandleInput(_event);
    }

    public override void PhysicProcess(double delta)
    {
        Area2D ladderGridArea = Entity.InteractionArea.IsOverlappingAreaWithLayer(GameCollisionLayer.LadderGrid);
        if (
            (Entity.Cache.JumpPressed || ladderGridArea == null)
        )
        {
            Entity.StateChanger.ChangeState(Player.State.Move);
            return;
        }

        ProcessKey();
        ProcessTimelessKey();

        float verticalDirection = Entity.Cache.VerticalDirection;
        if (verticalDirection == 0)
        {
            Entity.Velocity = Entity.Velocity.MoveToward(
                Entity.Velocity with {Y = 0},
                Entity.MoveSpeed
            );
        }
        else
        {
            Entity.Velocity = Entity.Velocity with {Y = verticalDirection * Entity.MoveSpeed};
        }

        float horizontalDirection = Entity.Cache.HorizontalDirection;
        if (horizontalDirection == 0)
        {
            Entity.Velocity = Entity.Velocity.MoveToward(
                Entity.Velocity with {X = 0},
                Entity.MoveSpeed
            );
        }
        else
        {
            Entity.Velocity = Entity.Velocity with {X = horizontalDirection * Entity.MoveSpeed};
        }

        Entity.MoveAndSlide();
    }
}