using System;

namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public CollectableSystemRewindData[] CollectableSystemRewindDatas { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public EnemySpacetimeRewindData[] EnemySpacetimeRewindDatas { get; set; }
    public JumpingEnemyRewindData[] JumpingEnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }
    public LockedDoorRewindData[] LockedDoorRewindDatas { get; set; }
    public OneWayPlatformRewindData[] OneWayPlatformRewindDatas { get; set; }
    public CannonBallRewindData[] CannonBallRewindDatas { get; set; }
    public CloudPlatformRewindData[] CloudPlatformRewindDatas { get; set; }
    public SwitcherRewindData[] SwitcherRewindDatas { get; set; }
    public SpawnerRewindData[] SpawnerRewindDatas { get; set; }
    public TrapSpawnerRewindData[] TrapSpawnerRewindDatas { get; set; }
    public ClawRewindData[] ClawRewindDatas { get; set; }
    public BoxRewindData[] BoxRewindDatas { get; set; }

    public WorldRewindData(RewindDataSource source)
    {
        CollectableSystemRewindDatas =
            CommonRewindData.CreateRewindData(source.CollectableSystem, s => new CollectableSystemRewindData(s));
        EnemyRewindDatas = CommonRewindData.CreateRewindData(source.Enemies, enemy => new EnemyRewindData(enemy));
        EnemySpacetimeRewindDatas =
            CommonRewindData.CreateRewindData(source.EnemySpacetimes, enemy => new EnemySpacetimeRewindData(enemy));
        JumpingEnemyRewindDatas =
            CommonRewindData.CreateRewindData(source.JumpingEnemies, enemy => new JumpingEnemyRewindData(enemy));
        KeyRewindDatas = CommonRewindData.CreateRewindData(source.Keys, key => new KeyRewindData(key));
        CannonBallRewindDatas =
            CommonRewindData.CreateRewindData(source.CannonBalls, ball => new CannonBallRewindData(ball));
        CloudPlatformRewindDatas =
            CommonRewindData.CreateRewindData(source.CloudPlatforms, entity => new CloudPlatformRewindData(entity));
        LockedDoorRewindDatas =
            CommonRewindData.CreateRewindData(source.LockedDoors, door => new LockedDoorRewindData(door));
        OneWayPlatformRewindDatas =
            CommonRewindData.CreateRewindData(source.OneWayPlatforms,
                platform => new OneWayPlatformRewindData(platform));
        SwitcherRewindDatas =
            CommonRewindData.CreateRewindData(source.Switchers, switcher => new SwitcherRewindData(switcher));
        SpawnerRewindDatas =
            CommonRewindData.CreateRewindData(source.Spawners, e => new SpawnerRewindData(e));
        TrapSpawnerRewindDatas =
            CommonRewindData.CreateRewindData(source.TrapSpawners, e => new TrapSpawnerRewindData(e));
        ClawRewindDatas = CommonRewindData.CreateRewindData(source.Claws, e => new ClawRewindData(e));
        BoxRewindDatas = CommonRewindData.CreateRewindData(source.Boxes, e => new BoxRewindData(e));
    }

    public void ApplyData(RewindDataSource source)
    {
        CommonRewindData.ApplyRewindData(source.CollectableSystem, CollectableSystemRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));

        CommonRewindData.ApplyRewindData(source.Enemies, EnemyRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.EnemySpacetimes, EnemySpacetimeRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.JumpingEnemies, JumpingEnemyRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Keys, KeyRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.LockedDoors, LockedDoorRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.OneWayPlatforms, OneWayPlatformRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Switchers, SwitcherRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.CannonBalls, CannonBallRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.CloudPlatforms, CloudPlatformRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Spawners, SpawnerRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.TrapSpawners, TrapSpawnerRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Claws, ClawRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Boxes, BoxRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
    }
}