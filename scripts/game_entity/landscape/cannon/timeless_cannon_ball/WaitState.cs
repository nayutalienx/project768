using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.timeless_cannon_ball;

public class WaitState : State<TimelessCannonBall, TimelessCannonBall.State>
{
    public WaitState(TimelessCannonBall entity, TimelessCannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessCannonBall.State prevState)
    {
        Entity.HideBall();
        Entity.GlobalPosition = Entity.InitialPosition;
    }
}