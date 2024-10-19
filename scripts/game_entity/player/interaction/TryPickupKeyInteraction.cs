using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class TryPickupKeyInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public TryPickupKeyInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        if (!Entity.InteractionContext.HasKey &&
            eventContext.Key.CurrentState.StateEnum == Key.State.Unpicked)
        {
            Entity.InteractionContext.HasKey = true;
            Entity.InteractionContext.Key = eventContext.Key;
            Entity.InteractionContext.KeyInstanceId = eventContext.Key.GetInstanceId();

            eventContext.Key.StateChanger.ChangeState(Key.State.Picked);
        }
    }
}