using System.Linq;
using Godot;
using Godot.Collections;

namespace project768.scripts.game_entity.common.system;

public class SaveSystem
{
    public string GetSaveFilePath(SceneTree tree)
    {
        var sceneName = tree.CurrentScene.SceneFilePath.Split("/").Last().Replace(".tscn", "");

        return $"user://{sceneName}.json";
    }

    public void SaveGame(SceneTree tree)
    {
        var filePath = GetSaveFilePath(tree);
        GD.Print($"SaveSystem: save to file {filePath}");

        using var saveFile = FileAccess.Open(filePath, FileAccess.ModeFlags.Write);

        var saveNodes = tree.GetNodesInGroup("persist");
        foreach (Node saveNode in saveNodes)
        {
            if (!saveNode.HasMethod("Save"))
            {
                GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
                continue;
            }

            var entity = saveNode as IPersistentEntity;
            var nodeData = entity.Save();
            var jsonString = Json.Stringify(nodeData);
            GD.Print($"SaveSystem: save {jsonString}");
            saveFile.StoreLine(jsonString);
        }
    }

    public void LoadGame(SceneTree tree)
    {
        var filePath = GetSaveFilePath(tree);
        GD.Print($"SaveSystem: load from file {filePath}");

        if (!FileAccess.FileExists(filePath))
        {
            GD.Print($"SaveSystem: file {filePath} not found");
            return;
        }

        using var saveFile = FileAccess.Open(GetSaveFilePath(tree), FileAccess.ModeFlags.Read);

        var saveNodes = tree.GetNodesInGroup("persist");

        for (int i = 0; i < saveNodes.Count; i++)
        {
            var jsonString = saveFile.GetLine();
            GD.Print($"SaveSystem: load {jsonString}");
            var json = new Json();
            var parseResult = json.Parse(jsonString);
            if (parseResult != Error.Ok)
            {
                GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
                continue;
            }

            // Get the data from the JSON object.
            var nodeData = new Godot.Collections.Dictionary<string, Variant>((Godot.Collections.Dictionary) json.Data);

            var entity = saveNodes[i] as IPersistentEntity;
            entity.Load(nodeData);
        }
    }
}