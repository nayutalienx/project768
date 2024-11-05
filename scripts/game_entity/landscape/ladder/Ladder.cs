using Godot;
using project768.scripts.game_entity.common;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class Ladder : DynamicSprite
{
    public override void _Ready()
    {
        InitDynamicSprite(false);
        Area2D moveLadderArea = GetNode<Area2D>("Shape");
        moveLadderArea.BodyEntered += MoveLadderArea_BodyEntered;
        moveLadderArea.BodyExited += MoveLadderArea_BodyExited;
    }

    private void MoveLadderArea_BodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.LadderArea)
            {
                LadderEvent = new PlayerLadderEvent()
                {
                    JoinedLadderArea = true,
                    LadderGlobalPosition = GlobalPosition
                }
            });
        }
    }


    private void MoveLadderArea_BodyExited(Node2D body)
    {
        if (body is Player player)
        {
            player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.LadderArea)
            {
                LadderEvent = new PlayerLadderEvent()
                {
                    JoinedLadderArea = false,
                    LadderGlobalPosition = GlobalPosition
                }
            });
        }
    }
}