using Godot;


public interface ISpawnable
{
    public bool CanSpawn();
    public bool TrySpawn(Vector2 spawnPosition, Vector2 direction);
}