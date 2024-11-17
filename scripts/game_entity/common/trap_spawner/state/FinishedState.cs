using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.trap_spawner.state;

public class FinishedState : State<TrapSpawner, TrapSpawner.State>
{
    public FinishedState(TrapSpawner entity, TrapSpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TrapSpawner.State prevState)
    {
        if (Entity.Sprite2D != null)
        {
            Entity.Sprite2D.Visible = false;
        }
    }
}