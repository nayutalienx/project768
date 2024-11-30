using System;
using System.Linq;
using Godot;
using project768.scripts.common;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public partial class SpacetimeRewindPlayer : Node2D
{
    public static SpacetimeRewindPlayer Instance { get; set; }

    [Export] public Label RewindLabel;

    public RewindDataSource RewindDataSource { get; set; }

    private const int MaxStates = 60 * 60 * 3; // Adjust based on how much time you want to rewind

    public bool RecordingPaused { get; set; }

    private FixedSizeStack<WorldRewindData> worldStates = new(MaxStates);
    private FixedSizeStack<WorldRewindData> rewindedBuffer = new(MaxStates);

    private FixedSizeStack<PlayerRewindData> playerStates = new(MaxStates);
    private FixedSizeStack<PlayerRewindData> playerRewindedBuffer = new(MaxStates);

    private int rewindSpeed;
    public bool IsRewinding { get; set; }
    public Vector2 TimelineStartPos { get; set; }
    public Vector2 TimelineEndPos { get; set; }

    public float TimelineProgress => CalculateTimelineProgress(RewindDataSource.Player.GlobalPosition.X, TimelineStartPos.X, TimelineEndPos.X);
    
    public static float CalculateTimelineProgress(float playerPosX, float startPosX, float endPosX)
    {
        return Mathf.Clamp((playerPosX - startPosX) / (endPosX - startPosX), 0, 1);
    }
    
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

    public override void _Ready()
    {
        Instance = this;
        if (RewindLabel != null)
        {
            RewindLabel.Hide();
        }

        RewindDataSource = new RewindDataSource(GetTree());
        var children = GetChildren();
        var start = children.First() as Node2D;
        var end = children.Last() as Node2D;
        TimelineStartPos = start.GlobalPosition;
        TimelineEndPos = end.GlobalPosition;
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
        else //  recodrding
        {
            RecordState();
        }
    }

    private void RecordState()
    {
        if (!RecordingPaused)
        {
            playerStates.Push(new PlayerRewindData(RewindDataSource.Player));
            worldStates.Push(new WorldRewindData(RewindDataSource));
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
                if (!playerStates.IsEmpty)
                {
                    var lastPlayerState = playerStates.Pop();
                    lastPlayerState.ApplyData(RewindDataSource.Player);
                    playerRewindedBuffer.Push(lastPlayerState);

                    if (!worldStates.IsEmpty)
                    {
                        var lastState = worldStates.Pop();
                        lastState.ApplyData(RewindDataSource);
                        rewindedBuffer.Push(lastState);
                    }
                }
                else
                {
                    RewindFinished();
                    GD.Print("Rewind player zero rewind[backward]");
                }
            }
            else
            {
                if (playerRewindedBuffer.Count != 0)
                {
                    var futurePlayerState = playerRewindedBuffer.Pop();
                    futurePlayerState.ApplyData(RewindDataSource.Player);
                    playerStates.Push(futurePlayerState);

                    if (rewindedBuffer.Count != 0)
                    {
                        var futureState = rewindedBuffer.Pop();
                        futureState.ApplyData(RewindDataSource);
                        worldStates.Push(futureState);
                    }
                }
                else
                {
                    RewindFinished();
                    GD.Print("Rewind player zero rewind[forward]");
                }
            }
        }
    }

    private void RewindStarted()
    {
        if (!IsRewinding)
        {
            if (GetTree().IsPaused())
            {
                GetTree().SetPause(false);
                RecordingPaused = false;
            }

            IsRewinding = true;
            RewindSpeed = 1;

            playerRewindedBuffer.Flush();
            rewindedBuffer.Flush();

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