﻿using project768.scripts.common;
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

        Entity.Visible = false;

        DropKey();
    }

    private void DropKey()
    {
        if (Entity.InteractionContext.HasKey)
        {
            Entity.InteractionContext.Key.StateChanger.ChangeState(Key.State.Unpicked);
            Entity.InteractionContext.HasKey = false;
        }
    }
}