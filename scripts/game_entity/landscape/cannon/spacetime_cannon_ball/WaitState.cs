using project768.scripts.player;
using project768.scripts.rewind;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.spacetime_cannon_ball;

public class WaitState : State<SpacetimeCannonBall, SpacetimeCannonBall.State>
{
    public WaitState(SpacetimeCannonBall entity, SpacetimeCannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(SpacetimeCannonBall.State prevState)
    {
        Entity.HideBall();
    }

    public override void PhysicProcess(double delta)
    {
        if (
            !Entity.SpawnLocked && SpacetimeRewindPlayer.Instance.GetCurrentTimelinePosition() > Entity.SpawnTimeInPixel
        )
        {
            Entity.StateChanger.ChangeState(SpacetimeCannonBall.State.Move);
            Entity.SpawnLocked = true;
            return;
        }
        
        if(Entity.PlayerKilled && Player.Instance.GlobalPosition.X < Entity.PlayerKilledPosition.X)
        {
            Entity.StateChanger.ChangeState(SpacetimeCannonBall.State.Move);
            Entity.PlayerKilled = false;
            return;
        }
    }
}