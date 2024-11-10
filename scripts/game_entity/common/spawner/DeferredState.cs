using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.spawner;

public class DeferredState : State<EntitySpawner, EntitySpawner.State>
{
    public DeferredState(EntitySpawner entity, EntitySpawner.State stateEnum) : base(entity, stateEnum)
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