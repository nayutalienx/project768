using Godot;
using project768.scripts.game_entity.npc.enemy.interaction.data;
using project768.scripts.player;
using project768.scripts.player.interaction;

public partial class Spike : Line2D
{
    public override void _Ready()
    {
        CollisionPolygon2D collisionPolygon2D = GetNode<CollisionPolygon2D>("Area2D/CollisionPolygon2D");

        var linePoints = GetPoints();

        float height = 10;

        Vector2[] polyPoints = new Vector2[4]
        {
            linePoints[0],
            linePoints[1],
            linePoints[0] + new Vector2(0, height),
            linePoints[1] + new Vector2(0, height),
        };

        collisionPolygon2D.Polygon = polyPoints;

        GetNode<Area2D>("Area2D").BodyEntered += body =>
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