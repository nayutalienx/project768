using Godot;

namespace project768.scripts.game_entity.common;

public partial class DynamicSpritePolygon : Polygon2D
{
    public override void _Ready()
    {
        InitDynamicSprite();
    }

    public void InitDynamicSprite()
    {
        CollisionPolygon2D collisionPolygon2D = GetNode<CollisionPolygon2D>("Shape/CollisionPolygon2D");
        var polygon = GetPolygon();
        collisionPolygon2D.Polygon = polygon;
    }
}