using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cloud_platform;

public class WaitState : State<CloudPlatform, CloudPlatform.State>
{
    public WaitState(CloudPlatform entity, CloudPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(CloudPlatform.State prevState)
    {
        Entity.CallDeferred(nameof(Entity.HideCloud));
    }
    
}