using System;
using project768.scripts.common;
using project768.scripts.item;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class DeathState : State<Enemy, Enemy.State>
{
    public DeathState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        Entity.DisableCollision();
        Entity.HeadArea.DisableCollision();
        Entity.AttackArea.DisableCollision();
        Entity.DoorKeyPickerContext.PutEvent(DoorKeyEvent.Dropped);

        Entity.Visible = false;
    }
}