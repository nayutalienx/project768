using System;
using Godot;

namespace project768.scripts.game_entity.common;

public partial class DynamicSprite : Line2D
{
    public override void _Ready()
    {
        InitDynamicSprite();
    }

    public void InitDynamicSprite()
    {
        CollisionPolygon2D collisionPolygon2D = GetNode<CollisionPolygon2D>("Shape/CollisionPolygon2D");

        var linePoints = GetPoints();

        var polys = collisionPolygon2D.Polygon;

        float height = Math.Abs(polys[1].Y - polys[0].Y);
        GD.Print($"height: {height}");

        float halfHeight = height / 2f;


        polys[0] = linePoints[0] - new Vector2(0, halfHeight);
        polys[1] = linePoints[1] - new Vector2(0, halfHeight);
        polys[2] = linePoints[1] + new Vector2(0, halfHeight);
        polys[3] = linePoints[0] + new Vector2(0, halfHeight);

        collisionPolygon2D.Polygon = polys;
    }
}