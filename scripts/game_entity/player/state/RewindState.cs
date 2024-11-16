using System;
using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class RewindState : BasePlayerState
{
    public RewindState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        if (prevState == Player.State.Death)
        {
            Entity.Label.Text = "";
        }

        Entity.DisableCollision();
    }

    public override void PhysicProcess(double delta)
    {
        ProcessTimelessKey();
    }
}