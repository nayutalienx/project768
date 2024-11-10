using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.common;
using project768.scripts.game_entity.common.timeless_spawner;
using project768.scripts.state_machine;

public partial class TimelessEntitySpawner : StaticBody2D,
    IStateMachineEntity<TimelessEntitySpawner, TimelessEntitySpawner.State>
{
    public enum State
    {
        Normal,
        Deferred
    }

    [Export] public State InitialState = State.Normal;
    [Export] public float SpawnInterval = 1.0f;

    public List<ISpawnable> Entities = new();
    public Node2D SpawnPoint;

    public int RewindState { get; set; }

    public State<TimelessEntitySpawner, State> CurrentState { get; set; }
    public Dictionary<State, State<TimelessEntitySpawner, State>> States { get; set; }
    public StateChanger<TimelessEntitySpawner, State> StateChanger { get; set; }
    public Label TimerLabel { get; set; }
    public TimerManager TimerManager { get; set; }

    public override void _Ready()
    {
        States = new Dictionary<State, State<TimelessEntitySpawner, State>>()
        {
            {State.Normal, new NormalState(this, State.Normal)},
            {State.Deferred, new DeferredState(this, State.Deferred)},
        };
        StateChanger = new StateChanger<TimelessEntitySpawner, State>(this);

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
        TimerLabel.Text = $"st: {TimerManager.CurrentTime:0.00}";
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
}