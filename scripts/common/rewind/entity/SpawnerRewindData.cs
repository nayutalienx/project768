using project768.scripts.game_entity.npc.spawner;

namespace project768.scripts.rewind.entity;

public struct SpawnerRewindData
{
    public double Timer { get; set; }

    public SpawnerRewindData(EntitySpawner spawner)
    {
        Timer = spawner.TimerManager.CurrentTime;
    }

    public void ApplyData(EntitySpawner spawner)
    {
        spawner.TimerManager.CurrentTime = Timer;
    }
}