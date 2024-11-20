using System;
using Godot;

namespace project768.scripts.game_entity.common;

public partial class DynamicSpriteLine : Line2D
{
    public override void _Ready()
    {
        InitDynamicSprite();
    }

    public void InitDynamicSprite(bool horizontal = true)
    {
        CollisionPolygon2D collisionPolygon2D = GetNode<CollisionPolygon2D>("Shape/CollisionPolygon2D");

        var linePoints = GetPoints();

        var polys = collisionPolygon2D.Polygon;


        if (horizontal)
        {
            float len = Math.Abs(linePoints[1].X - linePoints[0].X);
            polys[2] = polys[1] + new Vector2(len, 0);
            polys[3] = polys[0] + new Vector2(len, 0);
        }
        else
        {
            float height = Math.Abs(linePoints[1].Y - linePoints[0].Y);
            polys[1] = polys[0] + new Vector2(0, height * -1);
            polys[2] = polys[3] + new Vector2(0, height * -1);
        }

        collisionPolygon2D.Polygon = polys;
    }
}