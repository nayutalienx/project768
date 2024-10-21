using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using project768.scripts.game_entity.landscape.cannon;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.player;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public class RewindDataSource
{
    public Player Player { get; set; }
    public Enemy[] Enemies { get; set; }
    public Key[] Keys { get; set; }
    public LockedDoor[] LockedDoors { get; set; }
    public OneWayPlatform[] OneWayPlatforms { get; set; }
    public Switcher[] Switchers { get; set; }
    public CannonBall[] CannonBalls { get; set; }
    public EntitySpawner[] Spawners { get; set; }

    public List<IRewindable> Rewindables = new();

    public RewindDataSource(SceneTree t)
    {
        Player = FindAndAddRewindables(t, "player")[0] as Player;
        Enemies = FindAndAddRewindables(t, "enemy").ConvertAll(o => o as Enemy).ToArray();
        Keys = FindAndAddRewindables(t, "key").ConvertAll(o => o as Key).ToArray();
        LockedDoors = FindAndAddRewindables(t, "door").ConvertAll(o => o as LockedDoor).ToArray();
        Switchers = FindAndAddRewindables(t, "switcher").ConvertAll(o => o as Switcher).ToArray();
        CannonBalls = FindAndAddRewindables(t, "cannon_ball").ConvertAll(o => o as CannonBall).ToArray();
        Spawners = FindAndAddRewindables(t, "spawner").ConvertAll(o => o as EntitySpawner).ToArray();

        OneWayPlatforms = FindAndAddRewindables(t, "one_way_platform")
            .ConvertAll(o => o as OneWayPlatform)
            .Where(platform => platform.AnimationPlayer != null)
            .ToArray();
        FindAndAddRewindables(t, "background_music");
    }

    private List<IRewindable> FindAndAddRewindables(
        SceneTree tree,
        StringName group
    )
    {
        List<IRewindable> list = new();
        foreach (var child in tree.GetNodesInGroup(group))
        {
            IRewindable rewindable = (IRewindable) child;
            if (rewindable == null)
            {
                GD.Print($"ERROR: object is not rewindable {group}");
                throw new Exception("Object is not rewindable");
            }

            list.Add(rewindable);
        }

        Rewindables.AddRange(list);
        GD.Print($"Found {group} {list.Count} rewindables. Rewindables len: {Rewindables.Count}");
        return list;
    }
}