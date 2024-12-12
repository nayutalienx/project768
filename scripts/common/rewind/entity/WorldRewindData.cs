﻿using System;

namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public PlayerRewindData PlayerRewindData { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }
    public LockedDoorRewindData[] LockedDoorRewindDatas { get; set; }
    public OneWayPlatformRewindData[] OneWayPlatformRewindDatas { get; set; }
    public CannonBallRewindData[] CannonBallRewindDatas { get; set; }
    public SwitcherRewindData[] SwitcherRewindDatas { get; set; }
    public SpawnerRewindData[] SpawnerRewindDatas { get; set; }

    public WorldRewindData(RewindDataSource source)
    {
        PlayerRewindData = new PlayerRewindData(source.Player);
        EnemyRewindDatas = CommonRewindData.CreateRewindData(source.Enemies, enemy => new EnemyRewindData(enemy));
        KeyRewindDatas = CommonRewindData.CreateRewindData(source.Keys, key => new KeyRewindData(key));
        CannonBallRewindDatas =
            CommonRewindData.CreateRewindData(source.CannonBalls, ball => new CannonBallRewindData(ball));
        LockedDoorRewindDatas =
            CommonRewindData.CreateRewindData(source.LockedDoors, door => new LockedDoorRewindData(door));
        OneWayPlatformRewindDatas =
            CommonRewindData.CreateRewindData(source.OneWayPlatforms,
                platform => new OneWayPlatformRewindData(platform));
        SwitcherRewindDatas =
            CommonRewindData.CreateRewindData(source.Switchers, switcher => new SwitcherRewindData(switcher));
        SpawnerRewindDatas =
            CommonRewindData.CreateRewindData(source.Spawners, e => new SpawnerRewindData(e));
    }

    public void ApplyData(RewindDataSource source)
    {
        PlayerRewindData.ApplyData(source.Player);

        CommonRewindData.ApplyRewindData(source.Enemies, EnemyRewindDatas,
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
        CommonRewindData.ApplyRewindData(source.Spawners, SpawnerRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
    }
}