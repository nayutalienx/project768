using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.common.trap_spawner.state;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;
using RewindState = project768.scripts.game_entity.common.trap_spawner.state.RewindState;

public partial class TrapSpawner :
    Node2D,
    IRewindable,
    IStateMachineEntity<TrapSpawner, TrapSpawner.State>

{
    public enum State
    {
        Idle,
        Used,
        Finished,
        Rewind
    }

    [Export] public float TimeShouldPassBeforeSpawnTriggered = 1.0f;
    public Area2D TrapArea { get; set; }
    public List<ISpawnable> Entities = new();
    public Node2D SpawnPoint;
    public Label Label;
    public Sprite2D Sprite2D;
    
    public TimerManager SpawnTimer { get; set; }
    
    public int RewindState { get; set; }
    public State<TrapSpawner, State> CurrentState { get; set; }
    public Dictionary<State, State<TrapSpawner, State>> States { get; set; }
    public StateChanger<TrapSpawner, State> StateChanger { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        States = new Dictionary<State, State<TrapSpawner, State>>()
        {
            {State.Idle, new IdleState(this, State.Idle)},
            {State.Used, new UsedState(this, State.Used)},
            {State.Finished, new FinishedState(this, State.Finished)},
            {State.Rewind, new RewindState(this, State.Rewind)}
        };
        StateChanger = new StateChanger<TrapSpawner, State>(this);

        SpawnTimer = new TimerManager(TimeShouldPassBeforeSpawnTriggered);

        foreach (Node child in GetChildren())
        {
            if (child.Name.Equals("spawn"))
            {
                SpawnPoint = child as Node2D;
            }

            if (child is ISpawnable spawnable)
            {
                Entities.Add(spawnable);
            }
        }

        if (SpawnPoint == null)
        {
            SpawnPoint = this;
        }

        Label = GetNodeOrNull<Label>("Label");
        Sprite2D = GetNodeOrNull<Sprite2D>("Sprite2D");
        TrapArea = GetNodeOrNull<Area2D>("Area2D");
        if (TrapArea != null)
        {
            TrapArea.BodyEntered += body => { CurrentState.OnBodyEntered(new CollisionBody("trap_area", body)); };
        }

        StateChanger.ChangeState(State.Idle);
    }

    public override void _PhysicsProcess(double delta)
    {
        CurrentState.PhysicProcess(delta);

        if (Label != null)
        {
            Label.Text = $"t: {SpawnTimer.CurrentTime}\n" +
                         $"s: {CurrentState.StateEnum}";
        }
    }

    public bool TrySpawnEntity()
    {
        foreach (var node2D in Entities)
        {
            var spawnable = (ISpawnable) node2D;
            if (spawnable.TrySpawn(
                    SpawnPoint.GlobalPosition,
                    new Vector2(
                        Mathf.Cos(SpawnPoint.GlobalRotation),
                        Mathf.Sin(SpawnPoint.GlobalRotation)
                    )))
            {
                return true;
            }
        }

        return false;
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
    }
}