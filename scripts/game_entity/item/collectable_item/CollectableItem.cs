using Godot;
using System;

public partial class CollectableItem : Area2D
{
    public ReferenceRect ReferenceRect { get; set; }
    public Label Label { get; set; }

    public override void _Ready()
    {
        ReferenceRect = GetNode<ReferenceRect>("Control/Panel/ReferenceRect");
        Label = GetNode<Label>("Control/Panel/Label");
    }
}