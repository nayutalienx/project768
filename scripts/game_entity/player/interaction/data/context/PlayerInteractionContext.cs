using Godot;
using project768.scripts.common.interaction;

namespace project768.scripts.player.interaction;

public class PlayerInteractionContext : InteractionContext
{
    public PlayerLadderContext LadderContext { get; set; } = new();
    public PlayerKeyContext KeyContext { get; set; } = new();
    public PlayerSwitcherContext SwitcherContext { get; set; } = new();
}