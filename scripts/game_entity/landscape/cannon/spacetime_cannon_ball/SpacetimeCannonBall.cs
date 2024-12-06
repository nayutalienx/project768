using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon.spacetime_cannon_ball;

public partial class SpacetimeCannonBall : Area2D,
    IRewindable,
    IStateMachineEntity<SpacetimeCannonBall, SpacetimeCannonBall.State>
{
    public enum State
    {
        Wait,
        Move,
        Rewind
    }

    public State<SpacetimeCannonBall, State> CurrentState { get; set; }
    public Dictionary<State, State<SpacetimeCannonBall, State>> States { get; set; }
    public StateChanger<SpacetimeCannonBall, State> StateChanger { get; set; }
    public int RewindState { get; set; }
    public Sprite2D Sprite { get; set; }
    public CpuParticles2D Particles { get; set; }
    
    [Export] public SpacetimePathFollow SpacetimePathFollow { get; set; }
    [Export] public float SpawnTimeInPixel = 1000.0f;

    public bool SpawnLocked = false;
    public bool PlayerKilled = false;
    public Vector2 PlayerKilledPosition;

    public bool BallHidden
    {
        get => ballHidden;
        set
        {
            if (value)
            {
                HideBall();
            }
            else
            {
                ShowBall();
            }
        }
    }

    private bool ballHidden = true;
    

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite2D");
        Particles = GetNode<CpuParticles2D>("CPUParticles2D");

        States = new Dictionary<State, State<SpacetimeCannonBall, State>>()
        {
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Move, new MoveState(this, State.Move)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<SpacetimeCannonBall, State>(this);

        BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("ball", body)); };
        AreaEntered += area => { CurrentState.OnBodyEntered(new CollisionBody("area", area)); };

        SpacetimePathFollow.SetLocalTimelineStart(SpawnTimeInPixel);
        StateChanger.ChangeState(State.Wait);
    }

    public void HideBall()
    {
        ballHidden = true;
        Sprite.Hide();
        Particles.SetEmitting(false);
    }

    public void ShowBall()
    {
        ballHidden = false;
        Sprite.Show();
        Particles.SetEmitting(true);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void OnRewindSpeedChanged(int speed)
    {
        if (speed == 0)
        {
            Particles.SetEmitting(false);
        }
        else
        {
            Particles.SetEmitting(true);
            Particles.SpeedScale = Mathf.Abs(speed);
        }
    }
    
}