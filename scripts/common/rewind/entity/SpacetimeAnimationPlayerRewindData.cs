using Godot;

namespace project768.scripts.rewind.entity;

public struct SpacetimeAnimationPlayerRewindData
{
    
    public bool ShouldPlay { get; set; }
    
    public Vector2 PlayerPositionWhenUsed { get; set; }
    
    public SpacetimeAnimationPlayerRewindData(SpacetimeAnimationPlayer player)
    {
        if (player == null)
        {
            ShouldPlay = false;
            PlayerPositionWhenUsed = Vector2.Zero;
            return;
        }
        ShouldPlay = player.ShouldPlay;
        PlayerPositionWhenUsed = player.PlayerPositionStartTimeline;
    }
    
    public void ApplyData(SpacetimeAnimationPlayer player)
    {
        if (player == null)
        {
            return;
        }
        player.ShouldPlay = ShouldPlay;
        player.PlayerPositionStartTimeline = PlayerPositionWhenUsed;
    }
    
}