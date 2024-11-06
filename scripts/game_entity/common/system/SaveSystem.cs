using System;
using System.Linq;
using Godot;
using Godot.Collections;
using project768.scripts.common;

namespace project768.scripts.game_entity.common.system;

public class SaveSystem : Singleton<SaveSystem>
{
    private Json json = new Json();

    public string GetSceneName(SceneTree tree)
    {
        var sceneName = tree.CurrentScene.SceneFilePath.Split("/").Last().Replace(".tscn", "");
        return sceneName;
    }

    public string GetSaveFilePath()
    {
        return $"user://db.cfg";
    }

    public void SaveGame(SceneTree tree)
    {
        var configFile = new ConfigFile();
        var saveFilePath = GetSaveFilePath();
        var section = GetSceneName(tree);
        configFile.Load(saveFilePath);

        var saveNodes = tree.GetNodesInGroup("persist");
        foreach (Node saveNode in saveNodes)
        {
            if (!saveNode.HasMethod("Save"))
            {
                GD.Print($"persistent node '{saveNode.Name}' is missing a Save() function, skipped");
                continue;
            }

            var entity = saveNode as IPersistentEntity;
            entity.Save(section, configFile);
        }

        configFile.Save(saveFilePath);
    }

    public void LoadGame(SceneTree tree)
    {
        var configFile = new ConfigFile();
        var saveFilePath = GetSaveFilePath();
        var section = GetSceneName(tree);
        Error err = configFile.Load(saveFilePath);
        if (err != Error.Ok)
        {
            return;
        }

        if (!configFile.HasSection(section))
        {
            return;
        }

        var saveNodes = tree.GetNodesInGroup("persist");~~
        for (int i = 0; i < saveNodes.Count; i++)
        {
            var entity = saveNodes[i] as IPersistentEntity;
            entity.Load(section, configFile);
        }
    }
}