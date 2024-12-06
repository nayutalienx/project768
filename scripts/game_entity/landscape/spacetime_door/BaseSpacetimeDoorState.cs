using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_door;

public class BaseSpacetimeDoorState : State<SpacetimeLockedDoor, SpacetimeLockedDoor.State>
{
    public BaseSpacetimeDoorState(SpacetimeLockedDoor entity, SpacetimeLockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }
    
}