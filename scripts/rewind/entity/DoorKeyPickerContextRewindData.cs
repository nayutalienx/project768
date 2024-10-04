using project768.scripts.item;

namespace project768.scripts.rewind.entity;

public struct DoorKeyPickerContextRewindData
{
    public bool HasKey { get; set; }

    public DoorKeyPickerContextRewindData(DoorKeyPickerContext ctx)
    {
        HasKey = ctx.HasKey;
    }

    public void ApplyData(DoorKeyPickerContext ctx)
    {
        ctx.HasKey = HasKey;
    }
}