using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.switcher;

public class InitialState : State<Switcher, Switcher.State>
{
    public InitialState(Switcher entity, Switcher.State stateEnum) : base(entity, stateEnum)
    {
    }
}