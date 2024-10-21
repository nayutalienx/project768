using Godot;
using project768.scripts.common;
using project768.scripts.rewind.entity;

namespace project768.scripts.game_entity.npc.spawner;

public partial class EntitySpawner : StaticBody2D, IRewindable
{
    public enum State
    {
        Normal,
        Rewind
    }

    [Export] public float SpawnInterval = 1.0f;

    [Export] public Node2D[] Characters;
    [Export] public Node2D SpawnPoint;

    public int RewindState { get; set; }
    public Label TimerLabel { get; set; }
    public TimerManager TimerManager { get; set; }

    public override void _Ready()
    {
        TimerManager = new TimerManager(SpawnInterval);
        TimerLabel = GetNode<Label>("Label");

        if (SpawnPoint == null)
        {
            SpawnPoint = this;
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        TimerLabel.Text = $"st: {TimerManager.CurrentTime:0.00}";

        if (RewindState == (int) State.Rewind)
        {
            return;
        }

        var end = TimerManager.Update(delta);
        if (end)
        {
            if (TrySpawnCharacter())
            {
                TimerManager.Reset();
            }
        }
    }

    public bool TrySpawnCharacter()
    {
        foreach (var node2D in Characters)
        {
            var spawnable = (ISpawnable) node2D;
            if (spawnable.TrySpawn(SpawnPoint.GlobalPosition))
            {
                return true;
            }
        }

        return false;
    }


    public void RewindStarted()
    {
        RewindState = (int) State.Rewind;
    }

    public void RewindFinished()
    {
        RewindState = (int) State.Normal;
    }

    public void OnRewindSpeedChanged(int speed)
    {
    }
}