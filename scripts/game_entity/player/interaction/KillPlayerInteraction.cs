using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class KillPlayerInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public KillPlayerInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        Entity.StateChanger.ChangeState(Player.State.Death);
    }
}