using Godot;

namespace project768.scripts.item;

public interface DoorKeyPicker
{
    public Vector2 Position { get; set; }
    public DoorKeyPickerContext DoorKeyPickerContext { get; set; }
}