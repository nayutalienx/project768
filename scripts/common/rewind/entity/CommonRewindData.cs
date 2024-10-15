using System;

namespace project768.scripts.rewind.entity;

public class CommonRewindData
{
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