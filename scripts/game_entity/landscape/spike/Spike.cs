using System.Transactions;
using Godot;
using project768.scripts.game_entity.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.game_entity.npc.timeless_enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class Spike : DynamicSprite
{
    public override void _Ready()
    {
        InitDynamicSprite();

        var shape = GetNode<Area2D>("Shape");
        shape.BodyEntered += body =>
        {
            if (body is Player player)
            {
                player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
                return;
            }

            if (body is Enemy enemy)
            {
                enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
            }
            
            if (body is TimelessEnemy timelessEnemy)
            {
                timelessEnemy.Interactor.Interact(new TimelessEnemyInteractionEvent(TimelessEnemyInteraction.KillEnemy));
            }
        };
    }
}