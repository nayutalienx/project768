using System;

namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public PlayerRewindData PlayerRewindData { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }
    public LockedDoorRewindData[] LockedDoorRewindDatas { get; set; }
    public OneWayPlatformRewindData[] OneWayPlatformRewindDatas { get; set; }

    public WorldRewindData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors,
        OneWayPlatform[] oneWayPlatforms)
    {
        PlayerRewindData = new PlayerRewindData(player);
        EnemyRewindDatas = CreateRewindData(enemies, enemy => new EnemyRewindData(enemy));
        KeyRewindDatas = CreateRewindData(keys, key => new KeyRewindData(key));
        LockedDoorRewindDatas = CreateRewindData(lockedDoors, door => new LockedDoorRewindData(door));
        OneWayPlatformRewindDatas = CreateRewindData(oneWayPlatforms, platform => new OneWayPlatformRewindData(platform));
    }

    public void ApplyData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors,
        OneWayPlatform[] oneWayPlatforms)
    {
        PlayerRewindData.ApplyData(player);
        
        ApplyRewindData(enemies, EnemyRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
        ApplyRewindData(keys, KeyRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
        ApplyRewindData(lockedDoors, LockedDoorRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
        ApplyRewindData(oneWayPlatforms, OneWayPlatformRewindDatas, (rewindData, entity) => rewindData.ApplyData(entity));
    }

    public static T[] CreateRewindData<T, TEntity>(TEntity[] entities, Func<TEntity, T> createDataFunc)
    {
        T[] rewindData = new T[entities.Length];
        for (var i = entities.Length - 1; i >= 0; i--)
        {
            rewindData[i] = createDataFunc(entities[i]);
        }

        return rewindData;
    }
    
    public static void ApplyRewindData<T, TData>(T[] entities, TData[] rewindDatas, Action<TData, T> applyFunc)
    {
        for (var i = 0; i < entities.Length; i++)
        {
            applyFunc(rewindDatas[i], entities[i]);
        }
    }
}