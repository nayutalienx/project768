using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class PlayerInteractionEvent : InteractionEvent<PlayerInteraction>
{
    public PlayerLadderEvent LadderEvent { get; set; }
    public PlayerKeyEvent KeyEvent { get; set; }
    public PlayerSwitcherEvent SwitcherEvent { get; set; }

    public PlayerInteractionEvent(PlayerInteraction interactionEnum) : base(interactionEnum)
    {
    }
}