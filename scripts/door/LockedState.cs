using project768.scripts.state_machine;

namespace project768.scripts.door;

public class LockedState : State<LockedDoor, LockedDoor.State>
{
    public LockedState(LockedDoor entity, LockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(LockedDoor.State prevState)
    {
        Entity.CollisionShape2D.SetDeferred("disabled", false);
        Entity.LockArea.SetDeferred("monitoring", true);
    }
}