using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class SwitcherAreaInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public SwitcherAreaInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        if (eventContext.SwitcherEvent.JoinedSwitcherArea)
        {
            Entity.InteractionContext.SwitcherContext.JoinedSwitcherArea = true;
            Entity.InteractionContext.SwitcherContext.Switcher = eventContext.SwitcherEvent.Switcher;
            Entity.InteractionContext.SwitcherContext.InstanceId = eventContext.SwitcherEvent.Switcher.GetInstanceId();
        }
        else
        {
            Entity.InteractionContext.SwitcherContext.JoinedSwitcherArea = false;
            Entity.InteractionContext.SwitcherContext.Switcher = null;
        }
    }
}