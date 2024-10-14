using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class LadderState : State<Player, Player.State>
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

        Entity.Position = Entity.Position with {X = Entity.Ladder.X};

        if (Entity.Cache.DownPressed)
        {
            Entity.Position = Entity.Position with {Y = Entity.Position.Y + 1};
        }

        Entity.MoveAndSlide();
    }
}