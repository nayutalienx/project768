using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.timeless_door;

public class UnlockedState : State<TimelessLockedDoor, TimelessLockedDoor.State>
{
    public UnlockedState(TimelessLockedDoor entity, TimelessLockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TimelessLockedDoor.State prevState)
    {
        Entity.CollisionShape2D.SetDeferred("disabled", true);
        Entity.LockArea.SetDeferred("monitoring", false);
        if (prevState == TimelessLockedDoor.State.Locked)
        {
            Entity.AnimationPlayer.Play("DoorOpen");
        }
    }
}