using Godot;
using project768.scripts.item;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public partial class Player :
    CharacterBody2D,
    DoorKeyPicker,
    Rewindable,
    IStateMachineEntity<Player, Player.State>
{
    [Export] public float JumpVelocity = -400.0f;

    [Export] public float MoveSpeed = 300.0f;

    [Export] public float PushForce = 80.0f;

    public DoorKeyPickerContext DoorKeyPickerContext { get; set; } = new();

    public enum State
    {
        Move,
        Ladder,
        Death,
        Rewind
    }

    public int RewindState { get; set; }
    public State<Player, State> CurrentState { get; set; }
    public State<Player, State>[] States { get; set; }
    public StateChanger<Player, State> StateChanger { get; set; }
    public Vector2 Ladder { get; set; }
    public PlayerCache Cache { get; set; }

    public override void _Ready()
    {
        States = new State<Player, State>[]
        {
            new MoveState(this, State.Move),
            new LadderState(this, State.Ladder),
            new DeathState(this, State.Death),
            new RewindState(this, State.Rewind),
        };
        StateChanger = new StateChanger<Player, State>(this);
        StateChanger.ChangeState(State.Move);
    }

    public override void _Input(InputEvent _event)
    {
        CurrentState.HandleInput(_event);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);
    }

    public void HandleInput(InputEvent _event)
    {
        var cache = Cache;
        cache.VerticalDirection = Input.GetAxis("ui_up", "ui_down");
        cache.HorizontalDirection = Input.GetAxis("ui_left", "ui_right");

        cache.UpPressed = _event.IsActionPressed("ui_up");
        cache.DownPressed = _event.IsActionPressed("ui_down");
        cache.LeftPressed = _event.IsActionPressed("ui_left");
        cache.RightPressed = _event.IsActionPressed("ui_right");
        cache.JumpPressed = _event.IsActionPressed("ui_accept");

        Cache = cache;
    }

    public void CleanCache()
    {
        Cache = new PlayerCache();
    }

    private void ApplyImpulseToRigidBodies()
    {
        // Player impulse to rigid bodies
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision2D c = GetSlideCollision(i);
            if (c.GetCollider() is RigidBody2D)
            {
                ((RigidBody2D) c.GetCollider()).ApplyCentralImpulse(
                    -c.GetNormal() * PushForce);
            }
        }
    }

    public void RewindStarted()
    {
        StateChanger.ChangeState(State.Rewind);
    }

    public void RewindFinished()
    {
        StateChanger.ChangeState((State) RewindState);
    }

    public void EnteredLadderArea(Ladder ladder)
    {
        Ladder = ladder.Position;
    }

    public void ExitedLadderArea()
    {
        StateChanger.ChangeState(State.Move);
        Ladder = Vector2.Zero;
    }

    public void EnteredEnemyAttackArea()
    {
        StateChanger.ChangeState(State.Death);
    }

    public void EnteredSpikeArea()
    {
        StateChanger.ChangeState(State.Death);
    }

    public void EnteredEnemyHeadArea()
    {
        Velocity = Velocity with {Y = JumpVelocity};
    }
}