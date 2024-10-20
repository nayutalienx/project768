using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class PlayerInteractionEvent : InteractionEvent<PlayerInteraction>
{
    public bool JoinedLadderArea { get; set; }
    public Vector2 Ladder { get; set; }
    public Key Key { get; set; }

    public PlayerInteractionEvent(PlayerInteraction interactionEnum) : base(interactionEnum)
    {
    }
}