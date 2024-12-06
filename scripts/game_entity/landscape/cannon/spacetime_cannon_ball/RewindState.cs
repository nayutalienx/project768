using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.spacetime_cannon_ball;

public class RewindState : State<SpacetimeCannonBall, SpacetimeCannonBall.State>
{
    public RewindState(SpacetimeCannonBall entity, SpacetimeCannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void OnBodyEntered(CollisionBody body)
    {
        
    }
}