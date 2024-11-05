using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.spawner;

public class NormalState : State<EntitySpawner, EntitySpawner.State>
{
    public NormalState(EntitySpawner entity, EntitySpawner.State stateEnum) : base(entity, stateEnum)
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