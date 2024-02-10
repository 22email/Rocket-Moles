using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class JsonDataService : IDataService
{
    public T LoadData<T>(string relativePath)
    {
        string path = Path.Combine(Application.persistentDataPath, relativePath);
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist");
            throw new FileNotFoundException($"File does not exist at {path}");
        }

        try
        {
            T data = JsonConvert.DeserializeObject<T>(path);
            return data;
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public bool SaveData<T>(string relativePath, T data)
    {
        string path = Path.Combine(Application.persistentDataPath, relativePath);
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllText(path, JsonConvert.SerializeObject(data, Formatting.Indented));
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"File not created due to {e.StackTrace}");
            return false;
        }
    }
}
