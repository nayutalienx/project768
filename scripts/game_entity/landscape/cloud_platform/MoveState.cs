using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cloud_platform;

public class MoveState : State<CloudPlatform, CloudPlatform.State>
{
    public MoveState(CloudPlatform entity, CloudPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(CloudPlatform.State prevState)
    {
        Entity.CallDeferred(nameof(Entity.ShowCloud));
    }

    public override void PhysicProcess(double delta)
    {
        Entity.GlobalTransform = Entity.GlobalTransform.Translated(Entity.Direction * (float) (Entity.Speed * delta));

        if (Entity.RayCast2D.IsColliding())
        {
            Entity.StateChanger.ChangeState(CloudPlatform.State.Wait);
        }
    }
}