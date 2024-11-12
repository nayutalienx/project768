using Godot;
using project768.scripts.game_entity.common;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class Ladder : DynamicSprite
{
    public override void _Ready()
    {
        InitDynamicSprite(false);
    }
}