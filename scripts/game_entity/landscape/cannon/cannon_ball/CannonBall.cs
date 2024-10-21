using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public partial class CannonBall : Area2D,
    IRewindable,
    ISpawnable,
    IStateMachineEntity<CannonBall, CannonBall.State>
{
    public float Speed { get; set; } = 300.0f;

    public enum State
    {
        Wait,
        Move,
        Rewind
    }

    public State<CannonBall, State> CurrentState { get; set; }
    public Dictionary<State, State<CannonBall, State>> States { get; set; }
    public StateChanger<CannonBall, State> StateChanger { get; set; }
    public int RewindState { get; set; }
    public Sprite2D Sprite { get; set; }
    public GpuParticles2D Particles { get; set; }

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
        Particles = GetNode<GpuParticles2D>("GPUParticles2D");

        States = new Dictionary<State, State<CannonBall, State>>()
        {
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Move, new MoveState(this, State.Move)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<CannonBall, State>(this);

        BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("ball", body)); };

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

    public bool TrySpawn(Vector2 spawnPosition)
    {
        if (CurrentState.StateEnum == State.Wait)
        {
            GlobalPosition = spawnPosition;
            StateChanger.ChangeState(State.Move);
            return true;
        }

        return false;
    }
}