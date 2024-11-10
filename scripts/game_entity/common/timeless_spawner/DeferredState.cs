using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.timeless_spawner;

public class DeferredState : State<TimelessEntitySpawner, TimelessEntitySpawner.State>
{
    public DeferredState(TimelessEntitySpawner entity, TimelessEntitySpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {

        if (Entity.CanSpawnEntity())
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
}