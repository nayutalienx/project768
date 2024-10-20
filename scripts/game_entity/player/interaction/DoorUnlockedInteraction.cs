using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class DoorUnlockedInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public DoorUnlockedInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        Entity.InteractionContext.KeyContext.Key.StateChanger.ChangeState(Key.State.Used);
        Entity.InteractionContext.KeyContext.HasKey = false;
    }
}