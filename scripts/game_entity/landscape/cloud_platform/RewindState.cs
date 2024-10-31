using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cloud_platform;

public class RewindState : State<CloudPlatform, CloudPlatform.State>
{
    public RewindState(CloudPlatform entity, CloudPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }
}