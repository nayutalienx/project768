using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_switcher;

public class RewindState : State<SpacetimeSwitcher, SpacetimeSwitcher.State>
{
    public RewindState(SpacetimeSwitcher entity, SpacetimeSwitcher.State stateEnum) : base(entity, stateEnum)
    {
    }
}