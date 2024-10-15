using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public class RewindState : State<CannonBall, CannonBall.State>
{
    public RewindState(CannonBall entity, CannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }
}