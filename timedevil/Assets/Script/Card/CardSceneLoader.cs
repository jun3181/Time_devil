using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardSceneLoader : MonoBehaviour
{
    public Transform parentUI; // �̹������� ���� �θ�
    public GameObject imagePrefab; // Resources���� �ҷ��� ��������Ʈ�� ���� ������

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
                    Debug.LogWarning("�̹��� ��ã��: " + item.name);
                }
            }
        }
    }
}
