using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.timeless_spawner;

public class NormalState : State<TimelessEntitySpawner, TimelessEntitySpawner.State>
{
    public NormalState(TimelessEntitySpawner entity, TimelessEntitySpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        var end = Entity.TimerManager.Update(delta);
        if (end)
        {
            if (Entity.TrySpawnEntity())
            {
                Entity.TimerManager.Reset();
            }
        }
    }
}