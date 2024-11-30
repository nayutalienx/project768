using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using project768.scripts.game_entity.landscape.cannon;
using project768.scripts.game_entity.landscape.cloud_platform;
using project768.scripts.game_entity.npc.spawner;
using project768.scripts.player;
using project768.scripts.rewind.entity;

namespace project768.scripts.rewind;

public class RewindDataSource
{
    public Player Player { get; set; }
    public CollectableSystem[] CollectableSystem { get; set; }
    public Enemy[] Enemies { get; set; }
    public EnemySpacetime[] EnemySpacetimes { get; set; }
    public JumpingEnemy[] JumpingEnemies { get; set; }
    public Key[] Keys { get; set; }
    public LockedDoor[] LockedDoors { get; set; }
    public OneWayPlatform[] OneWayPlatforms { get; set; }
    public Switcher[] Switchers { get; set; }
    public CannonBall[] CannonBalls { get; set; }
    public CloudPlatform[] CloudPlatforms { get; set; }
    public EntitySpawner[] Spawners { get; set; }
    public TrapSpawner[] TrapSpawners { get; set; }
    public Claw[] Claws { get; set; }
    public Box[] Boxes { get; set; }

    public List<IRewindable> Rewindables = new();

    public RewindDataSource(SceneTree t)
    {
        Player = FindAndAddRewindables(t, "player")[0] as Player;
        Enemies = FindAndAddRewindables(t, "enemy").ConvertAll(o => o as Enemy).ToArray();
        EnemySpacetimes = FindAndAddRewindables(t, "spacetime_enemy").ConvertAll(o => o as EnemySpacetime).ToArray();
        JumpingEnemies = FindAndAddRewindables(t, "jumping_enemy").ConvertAll(o => o as JumpingEnemy).ToArray();
        Keys = FindAndAddRewindables(t, "key").ConvertAll(o => o as Key).ToArray();
        LockedDoors = FindAndAddRewindables(t, "door").ConvertAll(o => o as LockedDoor).ToArray();
        Switchers = FindAndAddRewindables(t, "switcher").ConvertAll(o => o as Switcher).ToArray();
        CannonBalls = FindAndAddRewindables(t, "cannon_ball").ConvertAll(o => o as CannonBall).ToArray();
        CloudPlatforms = FindAndAddRewindables(t, "cloud_platform").ConvertAll(o => o as CloudPlatform).ToArray();
        Spawners = FindAndAddRewindables(t, "spawner").ConvertAll(o => o as EntitySpawner).ToArray();
        TrapSpawners = FindAndAddRewindables(t, "trap_spawner").ConvertAll(o => o as TrapSpawner).ToArray();
        Claws = FindAndAddRewindables(t, "claw").ConvertAll(o => o as Claw).ToArray();
        Boxes = FindAndAddRewindables(t, "box").ConvertAll(o => o as Box).ToArray();

        OneWayPlatforms = FindAndAddRewindables(t, "one_way_platform").ConvertAll(o => o as OneWayPlatform).ToArray();
        FindAndAddRewindables(t, "background_music");
        CollectableSystem = FindAndAddRewindables(t, "collectable_system").ConvertAll(o => o as CollectableSystem)
            .ToArray();
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