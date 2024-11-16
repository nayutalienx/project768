using project768.scripts.common;
using project768.scripts.rewind;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class DeathState : State<Player, Player.State>
{
    public DeathState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        Entity.DisableCollision();
        Entity.DeathStopTimer.Reset();
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.DeathStopTimer.IsExpired())
        {
            if (!Entity.GetTree().IsPaused())
            {
                Entity.GetTree().SetPause(true);
                RewindPlayer.Instance.RecordingPaused = true;
                Entity.Label.Text = "you dead. rewind via SHIFT";
            }
        }
        else
        {
            Entity.DeathStopTimer.Update(delta);
            
            Entity.Velocity += Entity.GetGravity() * (float) delta;
            Entity.MoveAndSlide();
        }
    }
}