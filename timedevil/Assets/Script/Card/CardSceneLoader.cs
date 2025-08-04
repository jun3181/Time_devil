using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSceneLoader : MonoBehaviour
{
    public Transform parentUI; // 이미지들을 붙일 부모
    public GameObject imagePrefab; // Resources에서 불러온 스프라이트를 담을 프리팹

    void Start()
    {
        ItemDataList itemList = JsonSaveManager.LoadAll();

        foreach (var item in itemList.items)
        {
            if (item.obtained)
            {
                Sprite img = Resources.Load<Sprite>("my_asset/" + item.name);
                if (img != null)
                {
                    GameObject go = Instantiate(imagePrefab, parentUI);
                    go.GetComponent<Image>().sprite = img;
                }
                else
                {
                    Debug.LogWarning("이미지 못찾음: " + item.name);
                }
            }
        }
    }
}
