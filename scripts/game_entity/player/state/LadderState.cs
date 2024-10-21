using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class LadderState : BasePlayerState
{
    public LadderState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void HandleInput(InputEvent _event)
    {
        Entity.HandleInput(_event);
    }

    public override void EnterState(Player.State prevState)
    {
        RecoverKeyOnEnterState(prevState);
        RecoverSwitcherOnEnterState(prevState);
        Entity.EnableCollision(Entity.OrigCollission);
        if (prevState == Player.State.Rewind)
        {
            Entity.CleanCache();
        }
    }

    public override void PhysicProcess(double delta)
    {
        if (
            (Entity.Cache.JumpPressed || Entity.Cache.LeftPressed || Entity.Cache.RightPressed)
        )
        {
            Entity.StateChanger.ChangeState(Player.State.Move);
            return;
        }

        ProcessKey();

        float direction = Entity.Cache.VerticalDirection;
        if (direction == 0)
        {
            Entity.Velocity = Entity.Velocity.MoveToward(
                Entity.Velocity with {Y = 0}, Entity.MoveSpeed
            );
        }
        else
        {
            Entity.Velocity = Entity.Velocity with {Y = direction * Entity.MoveSpeed};
        }

        Entity.GlobalPosition = Entity.GlobalPosition with
        {
            X = Entity.InteractionContext.LadderContext.LadderGlobalPosition.X
        };

        if (Entity.Cache.DownPressed)
        {
            Entity.GlobalPosition = Entity.GlobalPosition with {Y = Entity.GlobalPosition.Y + 1};
        }

        Entity.MoveAndSlide();
    }
}