using project768.scripts.game_entity.landscape.cloud_platform;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.timeless_cloud_platform;

public class MoveState : State<TimelessCloudPlatform, TimelessCloudPlatform.State>
{
    public MoveState(TimelessCloudPlatform entity, TimelessCloudPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessCloudPlatform.State prevState)
    {
        Entity.CallDeferred(nameof(Entity.ShowCloud));
    }

    public override void PhysicProcess(double delta)
    {
        Entity.GlobalTransform = Entity.GlobalTransform.Translated(Entity.Direction * (float) (Entity.Speed * delta));

        if (Entity.RayCast2D.IsColliding())
        {
            Entity.StateChanger.ChangeState(TimelessCloudPlatform.State.Wait);
        }
    }
}