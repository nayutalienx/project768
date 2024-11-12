using Godot;

namespace project768.scripts.game_entity.common;

public partial class SceneLoader : StaticBody2D
{
    [Export(PropertyHint.File, "*.tscn")] public string Path { get; set; }

    [Export] public int SpawnPositionIndex { get; set; }


    public void Load()
    {
        GetTree().ChangeSceneToFile(Path);
    }

    public void LoadDeferred()
    {
        CallDeferred(nameof(Load));
    }
}