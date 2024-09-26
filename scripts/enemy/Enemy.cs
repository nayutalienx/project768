using Godot;
using System;

public partial class Enemy : CharacterBody2D
{

	[Signal]
	public delegate void EnemyInteractEventHandler(
		EnemyEvent ladderEvent
	);

	public float JUMP_VELOCITY = -400.0f;

	[Export]
	public float SPEED = 300.0f;

	private RayCast2D fallRaycastLeft;
	private RayCast2D fallRaycastRight;

	private bool isDead;
	private int enemyDirection = 1;
	private bool lockDirection = false;
	private Timer lockDirectionTimer;


	public override void _Ready()
	{
		fallRaycastLeft = GetNode<RayCast2D>("FallRaycast_1");
		fallRaycastRight = GetNode<RayCast2D>("FallRaycast_2");

		GetNode<Area2D>("EnemyHeadArea").BodyEntered += EnemyHeadBodyEntered;
		GetNode<Area2D>("EnemyAttackArea").BodyEntered += EnemyAttackBodyEntered;

		lockDirectionTimer = GetNode<Timer>("DirectionTimer");
		lockDirectionTimer.Timeout += OnTimerFinish;
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
			!fallRaycastLeft.IsColliding() ||
			!fallRaycastRight.IsColliding()
			|| IsOnWall()
			)
		{
			if (IsOnFloor() && !lockDirection)
			{
				enemyDirection *= -1;
				lockDirection = true;
				lockDirectionTimer.Start();
			}
		}

		Velocity = Velocity with { X = enemyDirection * SPEED };

		MoveAndSlide();

	}

	private void OnTimerFinish()
	{
		lockDirection = false;
	}
}
