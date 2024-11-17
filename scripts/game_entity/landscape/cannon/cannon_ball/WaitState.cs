using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public class WaitState : State<CannonBall, CannonBall.State>
{
    public WaitState(CannonBall entity, CannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(CannonBall.State prevState)
    {
        Entity.GlobalPosition = Entity.InitialPosition;
        Entity.HideBall();
    }
}