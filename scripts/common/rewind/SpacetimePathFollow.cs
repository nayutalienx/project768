using Godot;
using System;
using project768.scripts.rewind;

public partial class SpacetimePathFollow : PathFollow2D
{
    private Label Label;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label = GetNodeOrNull<Label>("Label");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        ProgressRatio = SpacetimeRewindPlayer.Instance.TimelineProgress;
        if (Label != null)
        {
            Label.Text = $"{ProgressRatio}";
        }
    }
}