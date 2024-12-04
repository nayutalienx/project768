using Godot;
using System;

public partial class SpacetimeCollectableItem : CollectableItem
{
    
    public bool Picked { get; set; }
    public Vector2 PickedPosition { get; set; }
}
