using project768.scripts.common;
using project768.scripts.player.interaction;
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

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is project768.scripts.player.Player player &&
            player.InteractionContext.KeyContext.HasKey)
        {
            player.Interactor.Interact(
                new PlayerInteractionEvent(PlayerInteraction.UnlockedDoor)
            );
            Entity.StateChanger.ChangeState(LockedDoor.State.Unlocked);
        }
    }
}