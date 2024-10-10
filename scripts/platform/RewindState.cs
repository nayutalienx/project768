using project768.scripts.state_machine;

namespace project768.scripts.platform;

public class RewindState : State<OneWayPlatform, OneWayPlatform.State>
{
    public RewindState(OneWayPlatform entity, OneWayPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }
}