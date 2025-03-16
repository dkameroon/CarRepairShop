using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string savePath = Application.dataPath + "/save.json";

    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Game saved at: " + savePath);
        Debug.Log("Save json: " + json);
    }

    public static GameData Load()
    {
        if (!File.Exists(savePath))
        {
            Debug.LogWarning("⚠ Save file not found! Creating a new one...");

            GameData newGameData = new GameData(10000, 1, new List<SaveData>());

            Save(newGameData);

            return newGameData;
        }

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<GameData>(json);
    }
}