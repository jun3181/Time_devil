using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public string name;
    public bool obtained;
}

[System.Serializable]
public class ItemDataList
{
    public List<ItemData> items = new List<ItemData>();
}
