using System;

namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public PlayerRewindData PlayerRewindData { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }
    public LockedDoorRewindData[] LockedDoorRewindDatas { get; set; }
    public OneWayPlatformRewindData[] OneWayPlatformRewindDatas { get; set; }
    public CannonRewindData[] CannonRewindDatas { get; set; }
    public SwitcherRewindData[] SwitcherRewindDatas { get; set; }

    public WorldRewindData(RewindDataSource source)
    {
        PlayerRewindData = new PlayerRewindData(source.Player);
        CannonRewindDatas = CommonRewindData.CreateRewindData(source.Cannons, cannon => new CannonRewindData(cannon));
        EnemyRewindDatas = CommonRewindData.CreateRewindData(source.Enemies, enemy => new EnemyRewindData(enemy));
        KeyRewindDatas = CommonRewindData.CreateRewindData(source.Keys, key => new KeyRewindData(key));
        LockedDoorRewindDatas = CommonRewindData.CreateRewindData(source.LockedDoors, door => new LockedDoorRewindData(door));
        OneWayPlatformRewindDatas =
            CommonRewindData.CreateRewindData(source.OneWayPlatforms, platform => new OneWayPlatformRewindData(platform));
        SwitcherRewindDatas =
            CommonRewindData.CreateRewindData(source.Switchers, switcher => new SwitcherRewindData(switcher));
    }

    public void ApplyData(RewindDataSource source)
    {
        PlayerRewindData.ApplyData(source.Player);

        CommonRewindData.ApplyRewindData(source.Cannons, CannonRewindDatas, (rewind, entity) => { rewind.ApplyData(entity); });
        CommonRewindData.ApplyRewindData(source.Enemies, EnemyRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Keys, KeyRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.LockedDoors, LockedDoorRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.OneWayPlatforms, OneWayPlatformRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(source.Switchers, SwitcherRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
    }
}