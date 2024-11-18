using System.Collections.Generic;
using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.rewind.entity;
using project768.scripts.state_machine;


public partial class EntitySpawner : StaticBody2D, IRewindable, IStateMachineEntity<EntitySpawner, EntitySpawner.State>
{
    public enum State
    {
        Normal,
        Deferred,
        Rewind
    }

    [Export] public State InitialState = State.Normal;
    [Export] public float SpawnInterval = 1.0f;
    [Export] public bool Debug = false;

    public List<ISpawnable> Entities = new();
    public Node2D SpawnPoint;

    public int RewindState { get; set; }

    public State<EntitySpawner, State> CurrentState { get; set; }
    public Dictionary<State, State<EntitySpawner, State>> States { get; set; }
    public StateChanger<EntitySpawner, State> StateChanger { get; set; }
    public Label TimerLabel { get; set; }
    public TimerManager TimerManager { get; set; }

    public override void _Ready()
    {
        States = new Dictionary<State, State<EntitySpawner, State>>()
        {
            {State.Normal, new NormalState(this, State.Normal)},
            {State.Deferred, new DeferredState(this, State.Deferred)},
            {State.Rewind, new RewindState(this, State.Rewind)},
        };
        StateChanger = new StateChanger<EntitySpawner, State>(this);

        TimerManager = new TimerManager(SpawnInterval);
        TimerLabel = GetNode<Label>("Label");

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

        StateChanger.ChangeState(InitialState);
    }


    public override void _PhysicsProcess(double delta)
    {
        if (Debug)
        {
            TimerLabel.Text = $"st: {TimerManager.CurrentTime:0.00}";
        }

        CurrentState.PhysicProcess(delta);
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

    public bool CanSpawnEntity()
    {
        foreach (var spawnable in Entities)
        {
            if (spawnable.CanSpawn())
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
        StateChanger.ChangeState(InitialState);
    }

    public void OnRewindSpeedChanged(int speed)
    {
    }
}