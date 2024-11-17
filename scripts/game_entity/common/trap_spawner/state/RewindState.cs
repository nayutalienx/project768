using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.trap_spawner.state;

public class RewindState : State<TrapSpawner, TrapSpawner.State>
{
    public RewindState(TrapSpawner entity, TrapSpawner.State stateEnum) : base(entity, stateEnum)
    {
    }
}