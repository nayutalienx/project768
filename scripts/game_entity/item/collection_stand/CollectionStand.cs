using Godot;
using System;
using project768.scripts.game_entity.common.system;

public partial class CollectionStand : Node
{
    [Export] public string World { get; set; } = "none";

    private GridContainer GridContainer { get; set; }

    public override void _Ready()
    {
        GridContainer = GetNode<GridContainer>("GridContainer");
        
        foreach (int i in SaveSystem.Instance.GetPickedItemsFromWorld(World))
        {
            PackedScene templateScene = GD.Load("res://scenes/ui/collectable_item_template.tscn") as PackedScene;
            var panel = templateScene.Instantiate<Panel>();
            GridContainer.AddChild(panel);
            panel.GetNode<Label>("Label").Text = $"{i}";   
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}