using Godot;

namespace project768.scripts.game_entity.npc.spawner;

public interface ISpawnable
{
    public bool TrySpawn(Vector2 spawnPosition, Vector2 direction);
}