using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GetManager : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public GameObject talkPanel;
    public bool isAction;

    public void Action(GameObject scanObj)
    {
        Debug.Log($"Raycast hit: {scanObj.name} (Layer: {LayerMask.LayerToName(scanObj.layer)})"); // ✅ 이거 꼭 확인

        if (scanObj == null || talkText == null || talkPanel == null)
            return;

        if (isAction)
        {
            isAction = false;
        }
        else
        {
            isAction = true;
            scanObject = scanObj;
            talkText.text = "item get!!!\n";
            JsonSaveManager.SaveItem(scanObj.name);

        }

        talkPanel.SetActive(isAction);
    }

}
