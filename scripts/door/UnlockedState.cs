using project768.scripts.state_machine;

namespace project768.scripts.door;

public class UnlockedState : State<LockedDoor, LockedDoor.State>
{
    public UnlockedState(LockedDoor entity, LockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(LockedDoor.State prevState)
    {
        Entity.CollisionShape2D.SetDeferred("disabled", true);
        Entity.LockArea.SetDeferred("monitoring", false);
        if (prevState == LockedDoor.State.Locked)
        {
            Entity.AnimationPlayer.Play("DoorOpen");
        }
    }
}