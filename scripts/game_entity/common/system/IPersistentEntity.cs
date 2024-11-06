using Godot;

namespace project768.scripts.game_entity.common.system;

public interface IPersistentEntity
{
    void Save(string section, ConfigFile file);
    void Load(string section, ConfigFile file);
}