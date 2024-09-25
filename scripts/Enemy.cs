using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

	[Signal]
	public delegate void EnemyInteractEventHandler(
		LadderEvent ladderEvent
	);

	public float JUMP_VELOCITY = -400.0f;

	public float SPEED = 300.0f;

	private RayCast2D fallRaycastLeft;
	private RayCast2D fallRaycastRight;

	private bool isDead;
	private int enemyDirection = 1;


	public override void _Ready()
	{
		fallRaycastLeft = GetNode<RayCast2D>("FallRaycast_1");
		fallRaycastRight = GetNode<RayCast2D>("FallRaycast_2");

		GetNode<Area2D>("EnemyHeadArea").Connect(Area2D.SignalName.BodyEntered, Callable.From<Node2D>(EnemyHeadBodyEntered));

		GetNode<Area2D>("EnemyAttackArea").Connect(Area2D.SignalName.BodyEntered, Callable.From<Node2D>(EnemyAttackBodyEntered));
	}

	public void EnemyAttackBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{

			EmitSignal(
				SignalName.EnemyInteract,
				new EnemyEvent
				{
					KillPlayer = true
				});

		}
	}
	public void EnemyHeadBodyEntered(Node2D body)
	{
		if (body.IsInGroup("player"))
		{
			EmitSignal(
				SignalName.EnemyInteract,
				new EnemyEvent
				{
					DiedFromHeadJump = true
				});

			QueueFree();
		}
	}

	public override void _PhysicsProcess(double delta)
	{

		if (isDead)
		{
			return;
		}

		if (!IsOnFloor())
		{
			Velocity += GetGravity() * (float)delta;
		}

		if (
			!fallRaycastLeft.IsColliding() || !fallRaycastRight.IsColliding() ||
			IsOnWall()
			)
		{

			if (IsOnFloor())
			{
				enemyDirection *= -1;
			}
		}

		Velocity = Velocity with { X = enemyDirection * SPEED };

		MoveAndSlide();

	}
}
