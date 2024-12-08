using Godot;
using System;
using project768.scripts.player;
using project768.scripts.rewind;

public partial class SpacetimeAnimationPlayer : AnimationPlayer
{
    [Export] public string AnimationNameOnPlay { get; set; }
    [Export] public float TimelineLengthInPixel { get; set; } = 1000.0f;
    public bool ShouldPlay { get; set; } = false;

    public Label Label;
    public Vector2 PlayerPositionStartTimeline { get; set; }


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Label = GetNodeOrNull<Label>("Label");
        SetCurrentAnimation(AnimationNameOnPlay);
        SpeedScale = 0.0f;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (ShouldPlay)
        {
            float timelineProgress = SpacetimeRewindPlayer.CalculateTimelineProgressNoClamp(
                Player.Instance.GlobalPosition.X,
                PlayerPositionStartTimeline.X,
                PlayerPositionStartTimeline.X + TimelineLengthInPixel
            );

            float timelineProgressClap = Mathf.Clamp(timelineProgress, 0.0f, 0.98f);

            Seek(Mathf.Lerp(0.0f, GetCurrentAnimationLength(), timelineProgressClap), true);
        }
        
        if (Label != null)
        {
            Label.Text = $"ShouldPlay: {ShouldPlay}\n";
        }
    }
}