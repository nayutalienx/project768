using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using project768.scripts.common;

namespace project768.scripts.game_entity.common.system;

public class SaveSystem : Singleton<SaveSystem>
{
    private ConfigFile CachedConfig = null;

    public string SavePath
    {
        get => GetSaveFilePath();
    }

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
        var configFile = LoadSave();
        var section = GetSceneName(tree);

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

        configFile.Save(SavePath);
    }

    public void LoadGame(SceneTree tree)
    {
        var configFile = LoadSave();

        var section = GetSceneName(tree);

        if (!configFile.HasSection(section))
        {
            return;
        }

        var saveNodes = tree.GetNodesInGroup("persist");
        for (int i = 0; i < saveNodes.Count; i++)
        {
            var entity = saveNodes[i] as IPersistentEntity;
            entity.Load(section, configFile);
        }
    }

    public List<int> GetPickedItemsFromWorld(string worldPrefix)
    {
        var configFile = LoadSave();
        var result = new List<int>();
        int counter = 0;
        foreach (string section in configFile.GetSections())
        {
            if (section.StartsWith(worldPrefix))
            {
                foreach (var picked in configFile.GetValue(section, "picked").As<Array<bool>>())
                {
                    if (picked)
                    {
                        result.Add(counter);
                        counter++;
                    }
                }
            }
        }

        return result;
    }

    private ConfigFile LoadSave()
    {
        if (CachedConfig != null)
        {
            return CachedConfig;
        }

        var configFile = new ConfigFile();
        configFile.Load(SavePath);
        CachedConfig = configFile;
        return configFile;
    }
}