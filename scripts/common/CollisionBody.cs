using Godot;

namespace project768.scripts.common;

public class CollisionBody
{
    public StringName AreaName { get; set; }
    public Node2D Body { get; set; }

    public CollisionBody(StringName areaName, Node2D body)
    {
        AreaName = areaName;
        Body = body;
    }
}