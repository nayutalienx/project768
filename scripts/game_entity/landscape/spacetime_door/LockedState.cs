using project768.scripts.common;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.spacetime_door;

public class LockedState : State<SpacetimeLockedDoor, SpacetimeLockedDoor.State>
{
    public LockedState(SpacetimeLockedDoor entity, SpacetimeLockedDoor.State stateEnum) : base(entity, stateEnum)
    {
    }


    public override void EnterState(SpacetimeLockedDoor.State prevState)
    {
        Entity.CollisionShape2D.SetDeferred("disabled", false);
        Entity.LockArea.SetDeferred("monitoring", true);
        
        Entity.AnimationPlayer.SetCurrentAnimation("DoorOpen");
        Entity.AnimationPlayer.SpeedScale = 0.0f;
        Entity.AnimationPlayer.Seek(0.0f, true);
    }

    public override void Process(double delta)
    {
        if (Entity.TrackEnemies)
        {
            bool allDead = true;

            foreach (EnemySpacetime entityEnemy in Entity.Enemies)
            {
                if (entityEnemy.CurrentState.StateEnum != EnemySpacetime.State.Death)
                {
                    allDead = false;
                    break;
                }
            }

            if (allDead)
            {
                Entity.StateChanger.ChangeState(SpacetimeLockedDoor.State.Unlocked);
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
            Entity.PlayerPositionWhenDoorUnlocked = Player.Instance.GlobalPosition;

            player.Interactor.Interact(
                new PlayerInteractionEvent(PlayerInteraction.UnlockedDoor)
            );
            Entity.StateChanger.ChangeState(SpacetimeLockedDoor.State.Unlocked);
        }
    }
}