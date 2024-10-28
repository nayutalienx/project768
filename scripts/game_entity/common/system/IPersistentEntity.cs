using Godot;

namespace project768.scripts.game_entity.common.system;

public interface IPersistentEntity
{
    Godot.Collections.Dictionary<string, Variant> Save();
    void Load(Godot.Collections.Dictionary<string, Variant> data);
}