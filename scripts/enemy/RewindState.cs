using System;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class RewindState : State<Enemy, Enemy.State>
{
    public RewindState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        Entity.DisableCollision();
    }
}