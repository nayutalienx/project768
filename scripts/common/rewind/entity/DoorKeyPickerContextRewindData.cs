using project768.scripts.item;

namespace project768.scripts.rewind.entity;

public struct DoorKeyPickerContextRewindData
{
    public DoorKeyEvent DoorKeyEvent { get; set; }
    public bool HasKey { get; set; }
    public ulong KeyInstanceId { get; set; }

    public DoorKeyPickerContextRewindData(DoorKeyPickerContext ctx)
    {
        HasKey = ctx.HasKey;
        KeyInstanceId = ctx.KeyInstanceId;
        DoorKeyEvent = ctx.DoorKeyEvent;
    }

    public void ApplyData(DoorKeyPickerContext ctx)
    {
        ctx.HasKey = HasKey;
        ctx.KeyInstanceId = KeyInstanceId;
        ctx.DoorKeyEvent = DoorKeyEvent;
    }
}