using Godot;
using project768.scripts.game_entity.landscape.cannon;

namespace project768.scripts.rewind.entity;

public struct CannonBallRewindData
{
    public CannonBall.State State { get; set; }
    public Transform2D Transform2D { get; set; }

    public bool Visible { get; set; }

    public CannonBallRewindData(CannonBall cannonBall)
    {
        State = cannonBall.CurrentState.StateEnum;
        Transform2D = cannonBall.Transform;
        Visible = cannonBall.Visible;
    }

    public void ApplyData(CannonBall cannonBall)
    {
        cannonBall.Transform = Transform2D;
        cannonBall.RewindState = (int) State;
        cannonBall.Visible = Visible;
    }
}