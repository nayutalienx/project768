using project768.scripts.state_machine;

namespace project768.scripts.door;

public class RewindState : State<LockedDoor, LockedDoor.State>
{
    public RewindState(LockedDoor entity, LockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }
}