using Godot;
using project768.scripts.game_entity.landscape.cannon;

namespace project768.scripts.rewind.entity;

public struct CannonBallRewindData
{
    public CannonBall.State State { get; set; }
    public Transform2D Transform2D { get; set; }
    public bool Hidden { get; set; }

    public CannonBallRewindData(CannonBall cannonBall)
    {
        State = cannonBall.CurrentState.StateEnum;
        Transform2D = cannonBall.Transform;
        Hidden = cannonBall.BallHidden;
    }

    public void ApplyData(CannonBall cannonBall)
    {
        cannonBall.Transform = Transform2D;
        cannonBall.RewindState = (int) State;
        cannonBall.BallHidden = Hidden;
    }
}