using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private PlayerData playerData;

    private string path = "";
    private string persistantPath = "";

    private List<int> prevScores = new();

    void Start()
    {
        path = Path.Combine(Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json");
        persistantPath = Path.Combine(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json");
    }

    public void Save(float score)
    {
        if(File.Exists(path))
            prevScores = Load();

        playerData = new PlayerData(prevScores, score);
        playerData.SortScore();

        string json = JsonUtility.ToJson(playerData);
        Debug.Log("Save : " + json);

        using StreamWriter writer = new(path);
        writer.Write(json);
    }

    public List<int> Load()
    {
        using StreamReader reader = new(path);
        string json = reader.ReadToEnd();

        PlayerData data = JsonUtility.FromJson<PlayerData>(json);
        Debug.Log("Load : " + data.ToString());
        return data.scores;
    }
}
