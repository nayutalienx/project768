﻿using System;
using Godot;
using project768.scripts.common;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public partial class RewindPlayer : Node2D
{
    public static RewindPlayer Instance { get; set; }

    [Export] public Label RewindLabel;

    public RewindDataSource RewindDataSource { get; set; }

    private const int MaxStates = 60 * 60 * 3; // Adjust based on how much time you want to rewind

    private FixedSizeStack<WorldRewindData> worldStates = new(MaxStates);
    private FixedSizeStack<WorldRewindData> rewindedBuffer;
    private FixedSizeStack<PlayerRewindData> playerStates = new(MaxStates);
    private FixedSizeStack<PlayerRewindData> playerRewindedBuffer;

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
        foreach (IRewindable rewindable in RewindDataSource.Rewindables)
        {
            rewindable.OnRewindSpeedChanged(rewindSpeed);
        }
    }

    public bool RecordingPaused { get; set; }

    public override void _Ready()
    {
        Instance = this;
        if (RewindLabel != null)
        {
            RewindLabel.Hide();
        }

        RewindDataSource = new RewindDataSource(GetTree());
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

        if (_event.IsActionPressed("quit"))
        {
            GetTree().Quit();
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
                    lastState.ApplyData(RewindDataSource);
                    rewindedBuffer.Push(lastState);

                    var lastPlayerState = playerStates.Pop();
                    lastPlayerState.ApplyData(RewindDataSource.Player);
                    playerRewindedBuffer.Push(lastPlayerState);
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
                    futureState.ApplyData(RewindDataSource);
                    worldStates.Push(futureState);

                    var futurePlayerState = playerRewindedBuffer.Pop();
                    futurePlayerState.ApplyData(RewindDataSource.Player);
                    playerStates.Push(futurePlayerState);
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
        if (!RecordingPaused)
        {
            worldStates.Push(new WorldRewindData(RewindDataSource));
            playerStates.Push(new PlayerRewindData(RewindDataSource.Player));
        }
    }

    private void RewindStarted()
    {
        if (!IsRewinding)
        {
            if (GetTree().IsPaused())
            {
                GetTree().SetPause(false);
                Instance.RecordingPaused = false;
            }

            IsRewinding = true;
            RewindSpeed = 1;
            rewindedBuffer = new(MaxStates);
            playerRewindedBuffer = new(MaxStates);

            if (RewindLabel != null)
            {
                RewindLabel.Show();
            }

            foreach (IRewindable rewindable in RewindDataSource.Rewindables)
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

            foreach (IRewindable rewindable in RewindDataSource.Rewindables)
            {
                rewindable.RewindFinished();
            }
        }
    }

    private void ReloadFullScene()
    {
        var tree = GetTree();
        tree.ReloadCurrentScene();
    }
}