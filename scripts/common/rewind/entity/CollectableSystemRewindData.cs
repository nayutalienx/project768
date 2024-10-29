using Godot;

namespace project768.scripts.rewind.entity;

public struct CollectableSystemRewindData
{
    public bool[] Picked { get; set; }

    public CollectableSystemRewindData(CollectableSystem system)
    {
        Picked = new bool[system.Picked.Length];
        for (int i = 0; i < system.Picked.Length; i++)
        {
            Picked[i] = system.Picked[i];
        }
    }

    public void ApplyData(CollectableSystem system)
    {
        system.SyncItems(Picked);
    }
}