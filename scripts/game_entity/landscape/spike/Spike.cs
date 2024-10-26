using System.Transactions;
using Godot;
using project768.scripts.game_entity.common;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class Spike : DynamicSprite
{
    public override void _Ready()
    {
        InitDynamicSprite();

        GetNode<Area2D>("Shape").BodyEntered += body =>
        {
            if (body is Player player)
            {
                player.Interactor.Interact(new PlayerInteractionEvent(PlayerInteraction.KillPlayer));
                return;
            }

            if (body is Enemy enemy)
            {
                GD.Print("Spike kill enemy");
                enemy.Interactor.Interact(new EnemyInteractionEvent(EnemyInteraction.KillEnemy));
                return;
            }
        };
    }
}