using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.timeless_cloud_platform;

public class WaitState : State<TimelessCloudPlatform, TimelessCloudPlatform.State>
{
    public WaitState(TimelessCloudPlatform entity, TimelessCloudPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessCloudPlatform.State prevState)
    {
        Entity.CallDeferred(nameof(Entity.HideCloud));
    }
    
}