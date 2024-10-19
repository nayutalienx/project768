using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class EnteredLadderInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public EnteredLadderInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        Entity.InteractionContext.Ladder = eventContext.Ladder;
    }
}