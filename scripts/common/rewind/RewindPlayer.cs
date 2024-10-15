using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Godot;
using project768.scripts.common;
using project768.scripts.player;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public partial class RewindPlayer : Node2D
{
    [Export] public Label RewindLabel;

    public Player Player { get; set; }
    public Enemy[] Enemies { get; set; }
    public Key[] Keys { get; set; }
    public LockedDoor[] LockedDoors { get; set; }
    public OneWayPlatform[] OneWayPlatforms { get; set; }
    public Cannon[] Cannons { get; set; }

    public List<IRewindable> Rewindables = new();

    private const int MaxStates = 60 * 60 * 3; // Adjust based on how much time you want to rewind

    private FixedSizeStack<WorldRewindData> worldStates = new(MaxStates);
    private FixedSizeStack<WorldRewindData> rewindedBuffer;

    private int rewindSpeed;
    public bool IsRewinding { get; set; }

    public enum RewindMode
    {
        Backward,
        Stopped,
        Forward
    }

    public int RewindSpeed
    {
        get => rewindSpeed;
        set
        {
            if (RewindLabel != null)
            {
                RewindLabel.Text = $"{value}x";
            }

            rewindSpeed = value;
            NotifyRewindableSpeed();
        }
    }

    private void NotifyRewindableSpeed()
    {
        foreach (IRewindable rewindable in Rewindables)
        {
            rewindable.OnRewindSpeedChanged(rewindSpeed);
        }
    }

    public bool Paused { get; set; }

    public override void _Ready()
    {
        if (RewindLabel != null)
        {
            RewindLabel.Hide();
        }

        Player = FindAndAddRewindables("player")[0] as Player;
        Enemies = FindAndAddRewindables("enemy").ConvertAll(o => o as Enemy).ToArray();
        Keys = FindAndAddRewindables("key").ConvertAll(o => o as Key).ToArray();
        LockedDoors = FindAndAddRewindables("door").ConvertAll(o => o as LockedDoor).ToArray();
        Cannons = FindAndAddRewindables("cannon").ConvertAll(o => o as Cannon).ToArray();
        foreach (Cannon cannon in Cannons)
        {
            Rewindables.AddRange(cannon.CannonBallPool);
        }

        OneWayPlatforms = FindAndAddRewindables("one_way_platform")
            .ConvertAll(o => o as OneWayPlatform)
            .Where(platform => platform.AnimationPlayer != null)
            .ToArray();
        FindAndAddRewindables("background_music");
    }

    private List<IRewindable> FindAndAddRewindables(
        StringName group
    )
    {
        List<IRewindable> list = new();
        foreach (var child in GetTree().GetNodesInGroup(group))
        {
            IRewindable rewindable = (IRewindable) child;
            if (rewindable == null)
            {
                GD.Print($"ERROR: object is not rewindable {group}");
                throw new Exception("Object is not rewindable");
            }

            list.Add(rewindable);
        }

        Rewindables.AddRange(list);
        GD.Print($"Found {group} {list.Count} rewindables. Rewindables len: {Rewindables.Count}");
        return list;
    }

    public override void _Input(InputEvent _event)
    {
        if (IsRewinding)
        {
            if (_event.IsActionPressed("ui_up"))
            {
                RewindSpeed++;
            }

            if (_event.IsActionPressed("ui_down"))
            {
                RewindSpeed--;
            }
        }

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
            RecordState();
        }
    }

    public void RewindState()
    {
        RewindMode rewindMode = RewindMode.Backward;
        if (RewindSpeed == 0)
        {
            return;
        }

        if (RewindSpeed < 0)
        {
            rewindMode = RewindMode.Forward;
        }


        for (int i = 0; i < Math.Abs(RewindSpeed); i++)
        {
            if (rewindMode == RewindMode.Backward)
            {
                if (!worldStates.IsEmpty)
                {
                    var lastState = worldStates.Pop();
                    lastState.ApplyData(
                        Player, Enemies, Keys, LockedDoors, OneWayPlatforms, Cannons);
                    rewindedBuffer.Push(lastState);
                }
                else
                {
                    RewindFinished();
                    GD.Print("Rewind player zero rewind[backward]");
                }
            }
            else
            {
                if (rewindedBuffer.Count != 0)
                {
                    var futureState = rewindedBuffer.Pop();
                    futureState.ApplyData(Player, Enemies, Keys, LockedDoors, OneWayPlatforms, Cannons);
                    worldStates.Push(futureState);
                }
                else
                {
                    RewindFinished();
                    GD.Print("Rewind player zero rewind[forward]");
                }
            }
        }
    }


    public void RecordState()
    {
        if (!Paused)
        {
            worldStates.Push(new WorldRewindData(
                Player,
                Enemies,
                Keys,
                LockedDoors,
                OneWayPlatforms,
                Cannons));
        }
    }

    private void RewindStarted()
    {
        if (!IsRewinding)
        {
            IsRewinding = true;
            RewindSpeed = 1;
            rewindedBuffer = new(MaxStates);

            if (RewindLabel != null)
            {
                RewindLabel.Show();
            }

            foreach (IRewindable rewindable in Rewindables)
            {
                rewindable.RewindStarted();
            }
        }
    }

    private void RewindFinished()
    {
        if (IsRewinding)
        {
            IsRewinding = false;
            RewindSpeed = 1;

            if (RewindLabel != null)
            {
                RewindLabel.Hide();
            }

            foreach (IRewindable rewindable in Rewindables)
            {
                rewindable.RewindFinished();
            }
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