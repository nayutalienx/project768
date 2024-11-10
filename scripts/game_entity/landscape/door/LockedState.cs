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

    public override void Process(double delta)
    {
        if (Entity.TrackEnemies)
        {
            bool allDead = true;

            foreach (Enemy entityEnemy in Entity.Enemies)
            {
                if (entityEnemy.CurrentState.StateEnum != Enemy.State.Death)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                Entity.StateChanger.ChangeState(LockedDoor.State.Unlocked);
            }
        }
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is project768.scripts.player.Player player &&
            (
                player.InteractionContext.KeyContext.HasKey ||
                player.InteractionContext.TimelessKeyContext.HasKey
            ))
        {
            player.Interactor.Interact(
                new PlayerInteractionEvent(PlayerInteraction.UnlockedDoor)
            );
            Entity.StateChanger.ChangeState(LockedDoor.State.Unlocked);
        }
    }
}