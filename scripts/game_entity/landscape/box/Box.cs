using Godot;
using project768.scripts.common;
using project768.scripts.rewind.entity;

public partial class Box :
    RigidBody2D,
    IRewindable
{
    public Label Label;

    public Vector2 PausedRewindLinearVelocity;
    public float PausedRewindAngularVelocity;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label = GetNode<Label>("Label");
    }


    public int RewindState { get; set; }

    public override void _PhysicsProcess(double delta)
    {
        Label.Text = $"v: {LinearVelocity}\n" +
                     $"f: {Freeze}\n" +
                     $"s: {Sleeping}";
    }

    public void RewindStarted()
    {
    }

    public void RewindFinished()
    {
    }

    public void OnRewindSpeedChanged(int speed)
    {
        if (speed == 0)
        {
            PausedRewindLinearVelocity = LinearVelocity;
            PausedRewindAngularVelocity = AngularVelocity;
            GravityScale = 0.0f;
            SetLinearVelocity(Vector2.Zero);
            SetAngularVelocity(0);
        }
        else
        {
            GravityScale = 1.0f;
            SetLinearVelocity(PausedRewindLinearVelocity);
            SetAngularVelocity(PausedRewindAngularVelocity);
        }
    }
}