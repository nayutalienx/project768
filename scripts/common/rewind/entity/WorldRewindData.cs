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

    public WorldRewindData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors,
        OneWayPlatform[] oneWayPlatforms,
        Cannon[] cannons)
    {
        PlayerRewindData = new PlayerRewindData(player);
        CannonRewindDatas = CommonRewindData.CreateRewindData(cannons, cannon => new CannonRewindData(cannon));
        EnemyRewindDatas = CommonRewindData.CreateRewindData(enemies, enemy => new EnemyRewindData(enemy));
        KeyRewindDatas = CommonRewindData.CreateRewindData(keys, key => new KeyRewindData(key));
        LockedDoorRewindDatas = CommonRewindData.CreateRewindData(lockedDoors, door => new LockedDoorRewindData(door));
        OneWayPlatformRewindDatas =
            CommonRewindData.CreateRewindData(oneWayPlatforms, platform => new OneWayPlatformRewindData(platform));
    }

    public void ApplyData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors,
        OneWayPlatform[] oneWayPlatforms,
        Cannon[] cannons)
    {
        PlayerRewindData.ApplyData(player);

        CommonRewindData.ApplyRewindData(cannons, CannonRewindDatas, (rewind, entity) => { rewind.ApplyData(entity); });
        CommonRewindData.ApplyRewindData(enemies, EnemyRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(keys, KeyRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(lockedDoors, LockedDoorRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
        CommonRewindData.ApplyRewindData(oneWayPlatforms, OneWayPlatformRewindDatas,
            (rewindData, entity) => rewindData.ApplyData(entity));
    }
}