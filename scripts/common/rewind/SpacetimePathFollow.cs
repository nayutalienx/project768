using Godot;
using System;
using System.Collections.Generic;
using project768.scripts.player;
using project768.scripts.rewind;

public partial class SpacetimePathFollow : PathFollow2D
{
    public enum Mode
    {
        GlobalTimeline,
        ManualLocalTimeline
    }

    [Export] public Mode PathMode = Mode.GlobalTimeline;
    [Export] public float LocalTimelineLength = 1000.0f;
    [Export] public bool LoopTimeline = false;


    private Label Label;

    public float LocalTimelineStart;

    [ExportSubgroup("TimelineRange")]
    [Export(PropertyHint.Range, "0,1,")]
    public float TimelineProjectionMin { get; set; } = 0;

    [Export(PropertyHint.Range, "0,1,")] public float TimelineProjectionMax { get; set; } = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label = GetNodeOrNull<Label>("Label");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (PathMode == Mode.GlobalTimeline)
        {
            ProcessGlobalTimeline();
        }

        if (PathMode == Mode.ManualLocalTimeline)
        {
            ProcessManualLocalTimeline();
        }

        if (Label != null)
        {
            Label.Text = $"{ProgressRatio}";
        }
    }

    public void SetLocalTimelineStart(float localTimelineStart)
    {
        LocalTimelineStart = localTimelineStart;
    }

    private void ProcessManualLocalTimeline()
    {
        if (LoopTimeline)
        {
            var noClampProgress = SpacetimeRewindPlayer.CalculateTimelineProgressNoClamp(
                Player.Instance.GlobalPosition.X,
                LocalTimelineStart + SpacetimeRewindPlayer.Instance.TimelineStartPos.X,
                LocalTimelineStart + LocalTimelineLength + SpacetimeRewindPlayer.Instance.TimelineStartPos.X
            );
            var minClamped = Mathf.Clamp(noClampProgress, 0, 10);
            ProgressRatio = minClamped % 1;
        }
        else
        {
            ProgressRatio = SpacetimeRewindPlayer.CalculateTimelineProgress(
                Player.Instance.GlobalPosition.X,
                LocalTimelineStart + SpacetimeRewindPlayer.Instance.TimelineStartPos.X,
                LocalTimelineStart + LocalTimelineLength + SpacetimeRewindPlayer.Instance.TimelineStartPos.X
            );
        }
    }

    private void ProcessGlobalTimeline()
    {
        ProgressRatio = SpacetimeRewindPlayer.CalculateTimelineProgress(
            SpacetimeRewindPlayer.Instance.GlobalTimelineProgress,
            TimelineProjectionMin,
            TimelineProjectionMax
        );
    }
}