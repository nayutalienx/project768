using System.Collections.Generic;
using System.Runtime.InteropServices;
using Godot;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public partial class RewindPlayer : Node2D
{
    [Export] public player.Player Player;
    [Export] public Node2D EnemyArrayNode;
    [Export] public Node2D KeyArrayNode;

    public Enemy[] Enemies;
    public Key[] Keys;

    public List<Rewindable> Rewindables = new();

    private List<WorldRewindData> _states = new List<WorldRewindData>();
    private const int MaxStates = 60 * 60 * 3; // Adjust based on how much time you want to rewind

    public bool IsRewinding { get; set; }
    public bool Paused { get; set; }

    public override void _Ready()
    {
        Rewindables.Add(Player);
        
        List<Enemy> enemiesList = new();
        foreach (var child in EnemyArrayNode.GetChildren())
        {
            enemiesList.Add(child as Enemy);
            Rewindables.Add(child as Enemy);
        }

        Enemies = enemiesList.ToArray();

        List<Key> keysList = new();
        foreach (var child in KeyArrayNode.GetChildren())
        {
            keysList.Add(child as Key);
            Rewindables.Add(child as Key);
        }

        Keys = keysList.ToArray();


        int enemyBytes = Marshal.SizeOf(typeof(EnemyRewindData));
        int playerBytes = Marshal.SizeOf(typeof(PlayerRewindData));
        int worldBytes = Marshal.SizeOf(typeof(WorldRewindData));

        GD.Print($"enemy size: {enemyBytes} bytes");
        GD.Print($"player size: {playerBytes} bytes");
        GD.Print($"world size: {worldBytes} bytes");

        int enemySumBytesPerFrame = enemyBytes * Enemies.Length;
        GD.Print($"all enemy size per frame: {enemySumBytesPerFrame} bytes");

        int fullRewindBufferSize = (playerBytes + enemySumBytesPerFrame + worldBytes) * MaxStates;
        GD.Print(
            $"full rewind buffer size: {fullRewindBufferSize} bytes. {fullRewindBufferSize / 1024.0} kylobytes. {fullRewindBufferSize / 1024.0 / 1024.0} megabytes.");
    }

    public override void _Input(InputEvent _event)
    {
        if (_event.IsActionPressed("rewind"))
        {
            RewindStarted();
        }
        else if (_event.IsActionReleased("rewind"))
        {
            RewindFinished();
        }

        if (_event.IsActionPressed("reload"))
        {
            ReloadFullScene();
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsRewinding)
        {
            RewindState();
        }
        else
        {
            RecordState(new WorldRewindData(Player, Enemies, Keys));
        }
    }

    public void RewindState()
    {
        if (_states.Count > 0)
        {
            var lastState = _states[_states.Count - 1];
            _states.RemoveAt(_states.Count - 1);
            lastState.ApplyData(Player, Enemies, Keys);
        }
        else
        {
            RewindFinished();
            GD.Print("Rewind player zero rewind");
        }
    }


    public void RecordState(WorldRewindData state)
    {
        if (!Paused)
        {
            _states.Add(state);
            if (_states.Count > MaxStates)
            {
                _states.RemoveAt(0);
            }
        }
    }

    private void RewindStarted()
    {
        IsRewinding = true;
        foreach (Rewindable rewindable in Rewindables)
        {
            rewindable.RewindStarted();
        }
    }

    private void RewindFinished()
    {
        IsRewinding = false;
        foreach (Rewindable rewindable in Rewindables)
        {
            rewindable.RewindFinished();
        }
    }

    private void ReloadFullScene()
    {
        var world = GD.Load<PackedScene>("res://scenes/world.tscn");
        var root = GetTree().GetRoot();
        foreach (var child in root.GetChildren())
        {
            root.RemoveChild(child);
        }

        root.AddChild(world.Instantiate<Node2D>());
    }
}