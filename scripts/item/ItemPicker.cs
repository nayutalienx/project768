using Godot;

namespace project768.scripts.item;

public interface ItemPicker
{
    bool TryToPick(ItemEnum itemEnum);
}