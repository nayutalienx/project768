using Godot;

namespace project768.scripts.rewind.entity;

public struct BoxRewindData
{
    public Transform2D GlobalTransform { get; set; }
    public Vector2 LinearVelocity { get; set; }
    public float AngularVelocity { get; set; }

    public BoxRewindData(Box box)
    {
        GlobalTransform = box.GlobalTransform;
        LinearVelocity = box.LinearVelocity;
        AngularVelocity = box.AngularVelocity;
    }

    public void ApplyData(Box box)
    {
        PhysicsServer2D.BodySetState(
            box.GetRid(),
            PhysicsServer2D.BodyState.Transform,
            GlobalTransform
        );
        PhysicsServer2D.BodySetState(
            box.GetRid(),
            PhysicsServer2D.BodyState.LinearVelocity,
            LinearVelocity
        );
        PhysicsServer2D.BodySetState(
            box.GetRid(),
            PhysicsServer2D.BodyState.AngularVelocity,
            AngularVelocity
        );

        box.PausedRewindLinearVelocity = LinearVelocity;
        box.PausedRewindAngularVelocity = AngularVelocity;
    }
}