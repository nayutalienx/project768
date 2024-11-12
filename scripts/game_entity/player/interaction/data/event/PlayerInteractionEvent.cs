using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class PlayerInteractionEvent : InteractionEvent<PlayerInteraction>
{
    public PlayerKeyEvent KeyEvent { get; set; }
    public PlayerTimelessKeyEvent TimelessKeyEvent { get; set; }
    public PlayerInteractionEvent(PlayerInteraction interactionEnum) : base(interactionEnum)
    {
    }
}