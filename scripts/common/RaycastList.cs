using System.Collections.Generic;
using Godot;

namespace project768.scripts.common;

public class RaycastList
{
    public List<RayCast2D> RayCasts { get; set; } = new();


    public bool IsAllColliding()
    {
        foreach (RayCast2D rayCast2D in RayCasts)
        {
            if (!rayCast2D.IsColliding())
            {
                return false;
            }
        }

        return true;
    }
    
    public bool IsAnyColliding()
    {
        foreach (RayCast2D rayCast2D in RayCasts)
        {
            if (rayCast2D.IsColliding())
            {
                return true;
            }
        }

        return false;
    }

    public GodotObject GetAnyCollider()
    {
        foreach (RayCast2D rayCast2D in RayCasts)
        {
            var collider = rayCast2D.GetCollider();
            if (collider != null)
            {
                return collider;
            }
        }

        return null;
    }

    public void Add(RayCast2D rayCast2D)
    {
        RayCasts.Add(rayCast2D);
    }

    public void AddFromNode(Node2D node)
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is RayCast2D rayCast2D)
            {
                Add(rayCast2D);
            }
        }
    }
}