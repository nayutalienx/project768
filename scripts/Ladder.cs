using Godot;
using System;

public partial class Ladder : Node2D
{

	[Signal]
	public delegate void LadderInteractEventHandler(
		LadderEvent ladderEvent
	);

	private StaticBody2D ladderTop;

	public override void _Ready()
	{
		ladderTop = GetNode<StaticBody2D>("top");

		Area2D moveUpArea = GetNode<Area2D>("move_up_area");
		moveUpArea.BodyEntered += MoveUpZoneEntered;
		moveUpArea.BodyExited += MoveUpZoneExited;

		Area2D moveDownArea = GetNode<Area2D>("move_down_area");

		moveDownArea.BodyEntered += MoveDownZoneEntered;
		moveDownArea.BodyExited += MoveDownZoneExited;
	}

	public void MoveUpZoneEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(
				SignalName.LadderInteract,
				new LadderEvent
				{
					NearLadder = true,
					Position = Position,
					LadderTop = ladderTop
				});
		}
	}

	public void MoveUpZoneExited(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(
				SignalName.LadderInteract,
				new LadderEvent
				{
					NearLadder = false,
					Position = Position,
					LadderTop = ladderTop
				});
		}
	}


	public void MoveDownZoneEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(
				SignalName.LadderInteract,
				new LadderEvent
				{
					NearLadder = true,
					Position = Position,
					LadderTop = ladderTop
				});
		}
	}

	public void MoveDownZoneExited(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(
				SignalName.LadderInteract,
				new LadderEvent
				{
					NearLadder = false,
					Position = Position,
					LadderTop = ladderTop
				});
		}
	}

}
