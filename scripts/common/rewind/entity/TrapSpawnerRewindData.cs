using project768.scripts.game_entity.npc.spawner;

namespace project768.scripts.rewind.entity;

public struct TrapSpawnerRewindData
{
    public TrapSpawner.State CurrentState;
    public bool Visible;
    public double Timer { get; set; }

    public TrapSpawnerRewindData(TrapSpawner spawner)
    {
        Timer = spawner.SpawnTimer.CurrentTime;
        CurrentState = spawner.CurrentState.StateEnum;
        Visible = spawner.Sprite2D.Visible;
    }

    public void ApplyData(TrapSpawner spawner)
    {
        spawner.SpawnTimer.CurrentTime = Timer;
        spawner.RewindState = (int) CurrentState;
        spawner.Sprite2D.Visible = Visible;
    }
}