namespace project768.scripts.rewind.entity;

public struct WorldRewindData
{
    public PlayerRewindData PlayerRewindData { get; set; }
    public EnemyRewindData[] EnemyRewindDatas { get; set; }
    public KeyRewindData[] KeyRewindDatas { get; set; }

    public WorldRewindData(player.Player player, Enemy[] enemies, Key[] keys)
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

        PlayerRewindData = new PlayerRewindData(player);
        EnemyRewindDatas = enemyRewindDatas;
        KeyRewindDatas = keyRewindDatas;
    }

    public void ApplyData(player.Player player, Enemy[] enemies, Key[] keys)
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
    }
}