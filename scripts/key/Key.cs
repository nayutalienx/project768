using Godot;
using project768.scripts.item;

public partial class Key : RigidBody2D
{
// Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        var pickerArea = GetNode<Area2D>("picker");
        pickerArea.BodyEntered += PickerArea_BodyEntered;
    }

    private void PickerArea_BodyEntered(Node2D body)
    {
        if (body is ItemPicker itemPicker)
        {
            if (itemPicker.TryToPick(ItemEnum.Key))
            {
                QueueFree();
            }
        }
    }
}