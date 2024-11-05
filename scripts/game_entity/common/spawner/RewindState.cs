using project768.scripts.state_machine;

namespace project768.scripts.game_entity.npc.spawner;

public class RewindState : State<EntitySpawner, EntitySpawner.State>
{
    public RewindState(EntitySpawner entity, EntitySpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void PhysicProcess(double delta)
    {
        base.PhysicProcess(delta);
    }
}