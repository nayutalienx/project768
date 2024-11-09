using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class TryPickupTimelessKeyInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public TryPickupTimelessKeyInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {

        if (Entity.InteractionContext.KeyContext.HasKey || Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            return;
        }

        if (
            eventContext.TimelessKeyEvent.Key.CurrentState.StateEnum == TimelessKey.State.Unpicked)
        {
            Entity.InteractionContext.TimelessKeyContext.HasKey = true;
            Entity.InteractionContext.TimelessKeyContext.Key = eventContext.TimelessKeyEvent.Key;

            eventContext.TimelessKeyEvent.Key.StateChanger.ChangeState(TimelessKey.State.Picked);
        }
    }
}