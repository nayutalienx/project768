using Godot;
using project768.scripts.rewind;

public partial class SpacetimeSpawner : StaticBody2D
{
    [Export] public float SpawnIntervalInPixel = 1000.0f;
    [Export] public bool Debug = false;
    public Label TimerLabel { get; set; }

    public float CurrentTimeInPixel;

    public override void _Ready()
    {
        TimerLabel = GetNode<Label>("Label");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Debug)
        {
            TimerLabel.Text = $"ct: {CurrentTimeInPixel}\n";
        }
        
        CurrentTimeInPixel = SpacetimeRewindPlayer.Instance.CalculateSpacetimeTime(SpawnIntervalInPixel);
        
    }
}