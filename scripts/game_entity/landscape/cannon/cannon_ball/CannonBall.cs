using Godot;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public partial class CannonBall : Area2D,
    IRewindable,
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
    public State<CannonBall, State>[] States { get; set; }
    public StateChanger<CannonBall, State> StateChanger { get; set; }
    public int RewindState { get; set; }

    public override void _Ready()
    {
        States = new State<CannonBall, State>[]
        {
            new WaitState(this, State.Wait),
            new MoveState(this, State.Move),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<CannonBall, State>(this);

        BodyEntered += body =>
        {
            GD.Print($"{Name} collided with {body.Name}");
            StateChanger.ChangeState(State.Wait);
        };

        StateChanger.ChangeState(State.Wait);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    public void RewindStarted()
    {
    }

    public void RewindFinished()
    {
    }

    public void OnRewindSpeedChanged(int speed)
    {
    }
}