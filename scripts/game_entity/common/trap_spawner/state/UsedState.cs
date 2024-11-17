using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.trap_spawner.state;

public class UsedState : State<TrapSpawner, TrapSpawner.State>
{
    public UsedState(TrapSpawner entity, TrapSpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TrapSpawner.State prevState)
    {
        if (prevState != TrapSpawner.State.Rewind)
        {
            Entity.SpawnTimer.Reset();
        }
    }

    public override void PhysicProcess(double delta)
    {
        if (Entity.SpawnTimer.IsExpired())
        {
            Entity.TrySpawnEntity();
            Entity.StateChanger.ChangeState(TrapSpawner.State.Finished);
        }
        else
        {
            Entity.SpawnTimer.Update(delta);
        }
    }
}