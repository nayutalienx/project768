using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.landscape.cannon.timeless_cannon_ball;
using project768.scripts.state_machine;

public partial class TimelessCannonBall : Area2D,
    ISpawnable,
    IStateMachineEntity<TimelessCannonBall, TimelessCannonBall.State>
{
    public float Speed { get; set; } = 300.0f;

    public enum State
    {
        Wait,
        Move
    }

    public State<TimelessCannonBall, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessCannonBall, State>> States { get; set; }
    public StateChanger<TimelessCannonBall, State> StateChanger { get; set; }
    public int RewindState { get; set; }
    public Sprite2D Sprite { get; set; }
    public Vector2 Direction { get; set; } = Vector2.Right;

    public CpuParticles2D Particles { get; set; }

    public Vector2 InitialPosition { get; set; }

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
        InitialPosition = GlobalPosition;
        Sprite = GetNode<Sprite2D>("Sprite2D");
        Particles = GetNode<CpuParticles2D>("CPUParticles2D");

        States = new Dictionary<State, State<TimelessCannonBall, State>>()
        {
            {State.Wait, new WaitState(this, State.Wait)},
            {State.Move, new MoveState(this, State.Move)},
        };
        StateChanger = new StateChanger<TimelessCannonBall, State>(this);

        BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("ball", body)); };
        AreaEntered += area => { CurrentState.OnBodyEntered(new CollisionBody("area", area)); };

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

    public bool CanSpawn()
    {
        return CurrentState.StateEnum == State.Wait;
    }

    public bool TrySpawn(Vector2 position, Vector2 direction)
    {
        if (CanSpawn())
        {
            GlobalPosition = position;
            Direction = direction;
            StateChanger.ChangeState(State.Move);
            return true;
        }

        return false;
    }
}