using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.cannon;

public partial class Cannon : StaticBody2D
{
    public TimerManager TimerManager { get; set; } = new TimerManager(5.0f);
    public Label TimerLabel { get; set; }

    public List<CannonBall> CannonBallPool { get; set; }

    public Node2D EmitPosition { get; set; }

    public override void _Ready()
    {
        TimerLabel = GetNode<Label>("Label");
        CannonBallPool = GetNode<Node2D>("ball_pool")
            .GetChildren().ToList()
            .ConvertAll(b => b as CannonBall)
            .ToList();
        EmitPosition = GetNode<Node2D>("emit_point");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _PhysicsProcess(double delta)
    {
        var end = TimerManager.Update(delta);
        if (end)
        {
            if (TrySpawnBall())
            {
                TimerManager.Reset();
            }
        }

        TimerLabel.Text = $"t: {TimerManager.CurrentTime:0.00}";
    }

    public bool TrySpawnBall()
    {
        foreach (CannonBall cannonBall in CannonBallPool)
        {
            if (cannonBall.CurrentState.StateEnum == CannonBall.State.Wait)
            {
                cannonBall.Transform = EmitPosition.Transform;
                cannonBall.StateChanger.ChangeState(CannonBall.State.Move);
                return true;
            }
        }

        return false;
    }
}