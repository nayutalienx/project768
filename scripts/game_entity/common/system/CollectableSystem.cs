using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Godot.Collections;
using project768.scripts.game_entity.common.system;
using project768.scripts.player;
using project768.scripts.rewind.entity;

public partial class CollectableSystem : Node2D, IPersistentEntity, IRewindable
{
    public List<CollectableItem> CollectableItems { get; set; } = new();
    public List<int> TimelessCollectableItems { get; set; } = new();
    public List<int> SpacetimeCollectableItems { get; set; } = new();
    public GridContainer GridContainer { get; set; }
    public int RewindState { get; set; }
    public bool[] Picked { get; set; }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GridContainer = GetNode<GridContainer>("CanvasLayer/Control/GridContainer");

        foreach (var child in GetChildren())
        {
            if (child is CollectableItem collectableItem)
            {
                CollectableItems.Add(collectableItem);
                if (child is TimelessCollectableItem timelessCollectableItem)
                {
                    TimelessCollectableItems.Add(CollectableItems.Count - 1);
                }

                if (child is SpacetimeCollectableItem spacetimeCollectableItem)
                {
                    SpacetimeCollectableItems.Add(CollectableItems.Count - 1);
                }
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

        Picked = GetPicked();
    }

    public override void _Process(double delta)
    {
        foreach (var item in SpacetimeCollectableItems)
        {
            if (CollectableItems[item] is SpacetimeCollectableItem spacetimeCollectableItem)
            {
                if (spacetimeCollectableItem.Picked && Player.Instance.GlobalPosition.X < spacetimeCollectableItem.PickedPosition.X)
                {
                    spacetimeCollectableItem.Picked = false;
                    Picked[item] = false;
                    SyncItemsRewind(Picked);
                }
            }
        }
    }

    private void OnItemCollected(int index, Node2D body)
    {
        Picked[index] = true;

        if (SpacetimeCollectableItems.Contains(index))
        {
            var spacetimeCollectableItem = CollectableItems[index] as SpacetimeCollectableItem;
            spacetimeCollectableItem.Picked = true;
            spacetimeCollectableItem.PickedPosition = Player.Instance.GlobalPosition;
        }

        SyncItems(Picked);
    }

    public void Save(string section, ConfigFile file)
    {
        file.SetValue(section, "picked", new Array<bool>(Picked));
    }

    public void Load(string section, ConfigFile file)
    {
        SyncItems(file.GetValue(section, "picked").As<Array<bool>>().ToArray());
    }

    public void SyncItemsRewind(bool[] picked)
    {
        for (var i = picked.Length - 1; i >= 0; i--)
        {
            if (!TimelessCollectableItems.Contains(i))
            {
                Picked[i] = picked[i];
            }
        }

        for (var i = 0; i < Picked.Length; i++)
        {
            SetItemPicked(i, Picked[i]);
        }
    }

    public void SyncItems(bool[] picked)
    {
        Picked = picked;
        for (var i = 0; i < Picked.Length; i++)
        {
            SetItemPicked(i, Picked[i]);
        }
    }

    public void SetItemPicked(int index, bool picked)
    {
        if (picked)
        {
            CollectableItems[index].SetVisible(false);
            CollectableItems[index].SetProcess(false);
            GridContainer.GetChild(index).GetNode<Label>("Label").Text = $"{index}";
        }
        else
        {
            CollectableItems[index].SetVisible(true);
            CollectableItems[index].SetProcess(true);
            GridContainer.GetChild(index).GetNode<Label>("Label").Text = "";
        }
    }

    private bool[] GetPicked()
    {
        bool[] picked = new bool[CollectableItems.Count];
        for (int i = 0; i < CollectableItems.Count; i++)
        {
            picked[i] = !CollectableItems[i].Visible;
        }

        return picked;
    }

    public void RewindStarted()
    {
    }

    public void RewindFinished()
    {
    }

    public void OnRewindSpeedChanged(int speed)
    {
    }
}