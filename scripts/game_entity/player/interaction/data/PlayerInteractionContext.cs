using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class PlayerInteractionContext : InteractionContext
{
    // Ladder
    public Vector2 Ladder { get; set; }
    
    // Key
    public bool HasKey { get; set; }
    public Key Key { get; set; }
    public ulong KeyInstanceId { get; set; }
}