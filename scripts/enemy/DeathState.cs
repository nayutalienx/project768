using System;
using project768.scripts.item;
using project768.scripts.state_machine;

namespace project768.scripts.enemy;

public class DeathState : State<Enemy, Enemy.State>
{
    private Tuple<uint, uint> originalEntityLayerMask;
    private Tuple<uint, uint> originalHeadAreaLayerMask;
    private Tuple<uint, uint> originalAttackAreaLayerMask;

    public DeathState(Enemy entity, Enemy.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Enemy.State prevState)
    {
        originalEntityLayerMask = DisableCollision(Entity);
        originalHeadAreaLayerMask = DisableCollision(Entity.HeadArea);
        originalAttackAreaLayerMask = DisableCollision(Entity.AttackArea);

        Entity.DoorKeyPickerContext.DoorKeyEvent = DoorKeyEvent.Dropped;
        Entity.Visible = false;
        base.EnterState(prevState);
    }

    public override void ExitState(Enemy.State prevState)
    {
        EnableCollision(Entity, originalEntityLayerMask);
        EnableCollision(Entity.HeadArea, originalHeadAreaLayerMask);
        EnableCollision(Entity.AttackArea, originalAttackAreaLayerMask);

        Entity.DoorKeyPickerContext.DoorKeyEvent = DoorKeyEvent.None;
        base.ExitState(prevState);
    }
}