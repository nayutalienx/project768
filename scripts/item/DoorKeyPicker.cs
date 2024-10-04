using Godot;

namespace project768.scripts.item;

public interface DoorKeyPicker
{
    public Transform2D Transform { get; set; }
    public DoorKeyPickerContext DoorKeyPickerContext { get; set; }
}