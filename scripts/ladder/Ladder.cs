using Godot;
using System;

public partial class Ladder : Node2D
{
    [Signal]
    public delegate void LadderInteractEventHandler(
        LadderEvent ladderEvent
    );

    public override void _Ready()
    {
        Area2D moveLadderArea = GetNode<Area2D>("move_ladder_area");
        moveLadderArea.BodyEntered += MoveLadderArea_BodyEntered;
        moveLadderArea.BodyExited += MoveLadderArea_BodyExited;
    }

    private void MoveLadderArea_BodyEntered(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            EmitSignal(
                SignalName.LadderInteract,
                new LadderEvent
                {
                    NearLadder = true,
                    Position = Position,
                });
        }
    }


    private void MoveLadderArea_BodyExited(Node2D body)
    {
        if (body.IsInGroup("player"))
        {
            EmitSignal(
                SignalName.LadderInteract,
                new LadderEvent
                {
                    NearLadder = false,
                    Position = Position,
                });
        }
    }
}