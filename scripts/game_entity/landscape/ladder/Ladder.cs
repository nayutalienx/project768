using Godot;
using project768.scripts.player;

public partial class Ladder : Node2D
{
    public override void _Ready()
    {
        Area2D moveLadderArea = GetNode<Area2D>("move_ladder_area");
        moveLadderArea.BodyEntered += MoveLadderArea_BodyEntered;
        moveLadderArea.BodyExited += MoveLadderArea_BodyExited;
    }

    private void MoveLadderArea_BodyEntered(Node2D body)
    {
        if (body is Player player)
        {
            player.EnteredLadderArea(this);
        }
    }


    private void MoveLadderArea_BodyExited(Node2D body)
    {
        if (body is Player player)
        {
            player.ExitedLadderArea();
        }
    }
}