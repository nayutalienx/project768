using Godot;
using project768.scripts.common.interaction;
using project768.scripts.game_entity.common;

namespace project768.scripts.player.interaction;

public class PlayerInteractionContext : InteractionContext
{
    public PlayerLadderContext LadderContext { get; set; } = new();
    public PlayerKeyContext KeyContext { get; set; } = new();
    public PlayerTimelessKeyContext TimelessKeyContext { get; set; } = new();
}