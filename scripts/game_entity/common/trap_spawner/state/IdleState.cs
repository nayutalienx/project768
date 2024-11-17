using project768.scripts.common;
using project768.scripts.player;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.common.trap_spawner.state;

public class IdleState : State<TrapSpawner, TrapSpawner.State>
{
    public IdleState(TrapSpawner entity, TrapSpawner.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(TrapSpawner.State prevState)
    {
        if (Entity.Sprite2D != null)
        {
            Entity.Sprite2D.Visible = true;
        }
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Player player)
        {
            Entity.StateChanger.ChangeState(TrapSpawner.State.Used);
        }
    }
}