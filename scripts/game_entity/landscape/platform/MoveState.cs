using Godot;
using project768.scripts.state_machine;

namespace project768.scripts.platform;

public class MoveState : State<OneWayPlatform, OneWayPlatform.State>
{
    public MoveState(OneWayPlatform entity, OneWayPlatform.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.PlatformMoveMode)
        {
            if (Entity.StopOnRaycast && Entity.RayCast2D.IsColliding())
            {
                return;
            }

            Entity.GlobalPosition += Entity.PlatformMoveVector * (float) delta;
        }
    }
}