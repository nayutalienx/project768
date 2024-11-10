using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class DoorUnlockedInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public DoorUnlockedInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        if (Entity.InteractionContext.KeyContext.HasKey)
        {
            Entity.InteractionContext.KeyContext.Key.StateChanger.ChangeState(Key.State.Used);
            Entity.InteractionContext.KeyContext.HasKey = false;
        }

        if (Entity.InteractionContext.TimelessKeyContext.HasKey)
        {
            Entity.InteractionContext.TimelessKeyContext.Key.StateChanger.ChangeState(TimelessKey.State.Used);
            Entity.InteractionContext.TimelessKeyContext.HasKey = false;
        }
    }
}