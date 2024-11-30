using project768.scripts.common;

namespace project768.scripts.game_entity.npc.enemy_spacetime.state;

public class RewindState : BaseEnemySpacetimeState
{
    public RewindState(EnemySpacetime entity, EnemySpacetime.State stateEnum) : base(entity, stateEnum)
    {
    }
    
    public override void EnterState(EnemySpacetime.State prevState)
    {
    }

    public override void PhysicProcess(double delta)
    {
        ProcessTimelessKey();
    }
}