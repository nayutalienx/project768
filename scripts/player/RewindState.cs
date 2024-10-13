using System;
using Godot;
using project768.scripts.common;
using project768.scripts.state_machine;

namespace project768.scripts.player;

public class RewindState : State<Player, Player.State>
{
    public RewindState(Player entity, Player.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(Player.State prevState)
    {
        Entity.DisableCollision();
    }
}