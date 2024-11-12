using System;
using Godot;

namespace project768.scripts.common;

public static class GodotExtensions
{
    public static Tuple<uint, uint> GetCollisionLayerMask(this CollisionObject2D collisionObject2D)
    {
        return new Tuple<uint, uint>(collisionObject2D.CollisionLayer, collisionObject2D.CollisionMask);
    }

    public static void DisableCollision(this CollisionObject2D entity)
    {
        entity.CollisionLayer = 0;
        entity.CollisionMask = 0;
    }

    public static void EnableCollision(this CollisionObject2D entity, Tuple<uint, uint> layerMask)
    {
        entity.CollisionLayer = layerMask.Item1;
        entity.CollisionMask = layerMask.Item2;
    }

    public static void SetRigidBodyEnabled(this RigidBody2D rb, bool enable)
    {
        var colShape = rb.GetNode<CollisionShape2D>("CollisionShape2D");

        if (enable)
        {
            rb.GravityScale = 1;
            rb.SetFreezeMode(RigidBody2D.FreezeModeEnum.Kinematic);
            rb.SetSleeping(false);
            rb.SetDeferred(nameof(rb.Freeze), false);

            colShape.SetDeferred(nameof(colShape.Disabled), false);
        }
        else
        {
            rb.GravityScale = 0;
            rb.SetLinearVelocity(Vector2.Zero);
            rb.SetAngularVelocity(0.0f);
            rb.SetSleeping(true);
            rb.SetFreezeMode(RigidBody2D.FreezeModeEnum.Static);
            rb.SetDeferred(nameof(rb.Freeze), true);

            colShape.SetDeferred(nameof(colShape.Disabled), true);
        }
    }

    public static Node2D IsOverlappingBodyWithLayer(this Area2D area, GameCollisionLayer gameLayer)
    {
        if (area.HasOverlappingBodies())
        {
            foreach (Node2D overlappingBody in area.GetOverlappingBodies())
            {
                if (overlappingBody is PhysicsBody2D body)
                {
                    if (body.CollisionLayer == (uint) gameLayer)
                    {
                        return body;
                    }
                }
            }
        }

        return null;
    }

    public static Area2D IsOverlappingAreaWithLayer(this Area2D area, GameCollisionLayer gameLayer)
    {
        if (area.HasOverlappingAreas())
        {
            foreach (Area2D overlappingArea in area.GetOverlappingAreas())
            {
                if (overlappingArea.CollisionLayer == (uint) gameLayer)
                {
                    return overlappingArea;
                }
            }
        }

        return null;
    }
}