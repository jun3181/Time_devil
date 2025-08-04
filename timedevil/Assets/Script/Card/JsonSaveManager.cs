using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonSaveManager
{
    static string path = Application.persistentDataPath + "/itemData.json";

    public static void SaveItem(string itemName)
    {
        ItemDataList dataList = LoadAll();
        // 이미 있는지 체크
        if (!dataList.items.Exists(i => i.name == itemName))
        {
            dataList.items.Add(new ItemData { name = itemName, obtained = true });
            string json = JsonUtility.ToJson(dataList, true);
            File.WriteAllText(path, json);
        }
    }

    public static ItemDataList LoadAll()
    {
        if (!File.Exists(path)) return new ItemDataList();
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<ItemDataList>(json);
    }
}
