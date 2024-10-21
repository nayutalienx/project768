using Godot;
using project768.scripts.game_entity.landscape.cannon;

namespace project768.scripts.rewind.entity;

public struct CannonBallRewindData
{
    public CannonBall.State State { get; set; }
    public Vector2 GlobalPosition { get; set; }
    public bool Hidden { get; set; }

    public CannonBallRewindData(CannonBall cannonBall)
    {
        State = cannonBall.CurrentState.StateEnum;
        GlobalPosition = cannonBall.GlobalPosition;
        Hidden = cannonBall.BallHidden;
    }

    public void ApplyData(CannonBall cannonBall)
    {
        cannonBall.GlobalPosition = GlobalPosition;
        cannonBall.RewindState = (int) State;
        cannonBall.BallHidden = Hidden;
    }
}