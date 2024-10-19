using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class ExitedLadderInteraction : Interaction<Player, PlayerInteractionEvent, PlayerInteraction>
{
    public ExitedLadderInteraction(Player entity) : base(entity)
    {
    }

    public override void Interact(PlayerInteractionEvent eventContext)
    {
        if (Entity.CurrentState.StateEnum == Player.State.Rewind)
        {
            return;
        }

        Entity.StateChanger.ChangeState(Player.State.Move);
        Entity.InteractionContext.Ladder = Vector2.Zero;
    }
}