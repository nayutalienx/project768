using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;
using project768.scripts.game_entity.common.system;

public partial class CollectableSystem : Node2D, IPersistentEntity
{
    public List<CollectableItem> CollectableItems { get; set; } = new();
    public GridContainer GridContainer { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GridContainer = GetNode<GridContainer>("CanvasLayer/Control/GridContainer");

        foreach (var child in GetChildren())
        {
            if (child is CollectableItem collectableItem)
            {
                CollectableItems.Add(collectableItem);
            }
        }

        for (int i = 0; i < CollectableItems.Count; i++)
        {
            PackedScene templateScene = GD.Load("res://scenes/ui/collectable_item_template.tscn") as PackedScene;
            GridContainer.AddChild(templateScene.Instantiate<Panel>());
            CollectableItems[i].Label.Text = $"{i}";
            var i1 = i;
            CollectableItems[i].BodyEntered += body => OnItemCollected(i1, body);
        }
    }

    private void OnItemCollected(int index, Node2D body)
    {
        CollectableItems[index].SetVisible(false);
        CollectableItems[index].SetProcess(false);
        GridContainer.GetChild(index).GetNode<Label>("Label").Text = $"{index}";
    }

    public Godot.Collections.Dictionary<string, Variant> Save()
    {
        bool[] picked = new bool[CollectableItems.Count];

        for (int i = 0; i < CollectableItems.Count; i++)
        {
            picked[i] = !CollectableItems[i].Visible;
        }

        return new Godot.Collections.Dictionary<string, Variant>()
        {
            {$"picked", new Array<bool>(picked)}
        };
    }

    public void Load(Godot.Collections.Dictionary<string, Variant> data)
    {
        var picked = data["picked"].As<Array<bool>>();
        for (var i = 0; i < picked.Count; i++)
        {
            if (picked[i])
            {
                CollectableItems[i].SetVisible(false);
                CollectableItems[i].SetProcess(false);
                GridContainer.GetChild(i).GetNode<Label>("Label").Text = $"{i}";
            }
        }
    }
}