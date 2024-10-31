using Godot;
using project768.scripts.game_entity.landscape.cannon;
using project768.scripts.game_entity.landscape.cloud_platform;

namespace project768.scripts.rewind.entity;

public struct CloudPlatformRewindData
{
    public CloudPlatform.State State { get; set; }
    public Vector2 GlobalPosition { get; set; }
    public bool Hidden { get; set; }

    public CloudPlatformRewindData(CloudPlatform cloudPlatform)
    {
        State = cloudPlatform.CurrentState.StateEnum;
        GlobalPosition = cloudPlatform.GlobalPosition;
        Hidden = cloudPlatform.CloudHidden;
    }

    public void ApplyData(CloudPlatform cloudPlatform)
    {
        cloudPlatform.GlobalPosition = GlobalPosition;
        cloudPlatform.RewindState = (int) State;
        cloudPlatform.CloudHidden = Hidden;
    }
}