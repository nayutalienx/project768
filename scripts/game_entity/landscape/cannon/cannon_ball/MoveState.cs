﻿using Godot;
using project768.scripts.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;
using project768.scripts.state_machine;

namespace project768.scripts.game_entity.landscape.cannon;

public class MoveState : State<CannonBall, CannonBall.State>
{
    public MoveState(CannonBall entity, CannonBall.State stateEnum) : base(entity, stateEnum)
    {
    }

    public override void EnterState(CannonBall.State prevState)
    {
        Entity.ShowBall();
    }

    public override void PhysicProcess(double delta)
    {
        Entity.GlobalTransform = Entity.GlobalTransform.Translated(Entity.Direction * (float) (Entity.Speed * delta));
    }

    public override void OnBodyEntered(CollisionBody body)
    {
        if (body.Body is Enemy enemy)
        {
            enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
        }

        if (body.Body is Player player)
        {
            player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
        }

        if (body.Body is TimelessEnemy timelessEnemy)
        {
            timelessEnemy.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
        }

        Entity.StateChanger.ChangeState(CannonBall.State.Wait);
    }
}