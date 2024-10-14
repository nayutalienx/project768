using project768.scripts.state_machine;

namespace project768.scripts.platform;

public class MoveState : State<OneWayPlatform, OneWayPlatform.State>
{
    public MoveState(OneWayPlatform entity, OneWayPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }
}