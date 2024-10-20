using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class TryPickupKeyInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public TryPickupKeyInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        if (!Entity.InteractionContext.KeyContext.HasKey &&
            eventContext.KeyEvent.Key.CurrentState.StateEnum == Key.State.Unpicked)
        {
            Entity.InteractionContext.KeyContext.HasKey = true;
            Entity.InteractionContext.KeyContext.Key = eventContext.KeyEvent.Key;
            Entity.InteractionContext.KeyContext.KeyInstanceId = eventContext.KeyEvent.Key.GetInstanceId();

            eventContext.KeyEvent.Key.StateChanger.ChangeState(Key.State.Picked);
        }
    }
}