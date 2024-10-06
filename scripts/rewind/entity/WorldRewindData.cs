namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public PlayerRewindData PlayerRewindData { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }
    public LockedDoorRewindData[] LockedDoorRewindDatas { get; set; }

    public WorldRewindData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors)
    {
        EnemyRewindData[] enemyRewindDatas = new EnemyRewindData[enemies.Length];
        for (var i = enemies.Length - 1; i >= 0; i--)
        {
            enemyRewindDatas[i] = new EnemyRewindData(enemies[i]);
        }

        KeyRewindData[] keyRewindDatas = new KeyRewindData[keys.Length];
        for (var i = keys.Length - 1; i >= 0; i--)
        {
            keyRewindDatas[i] = new KeyRewindData(keys[i]);
        }

        LockedDoorRewindData[] doorDatas = new LockedDoorRewindData[lockedDoors.Length];
        for (var i = lockedDoors.Length - 1; i >= 0; i--)
        {
            doorDatas[i] = new LockedDoorRewindData(lockedDoors[i]);
        }


        PlayerRewindData = new PlayerRewindData(player);
        EnemyRewindDatas = enemyRewindDatas;
        KeyRewindDatas = keyRewindDatas;
        LockedDoorRewindDatas = doorDatas;
    }

    public void ApplyData(
        player.Player player,
        Enemy[] enemies,
        Key[] keys,
        LockedDoor[] lockedDoors)
    {
        PlayerRewindData.ApplyData(player);
        for (var i = enemies.Length - 1; i >= 0; i--)
        {
            EnemyRewindDatas[i].ApplyData(enemies[i]);
        }

        for (var i = keys.Length - 1; i >= 0; i--)
        {
            KeyRewindDatas[i].ApplyData(keys[i]);
        }

        for (var i = 0; i < lockedDoors.Length; i++)
        {
            LockedDoorRewindDatas[i].ApplyState(lockedDoors[i]);
        }
    }
}