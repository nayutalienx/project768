using System;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class RewindState : State<Enemy, Enemy.State>
{
    private Tuple<uint, uint> origCollission;

    public RewindState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        origCollission = DisableCollision(Entity);
    }

    public override void ExitState(Enemy.State nextState)
    {
        EnableCollision(Entity, origCollission);
    }
}