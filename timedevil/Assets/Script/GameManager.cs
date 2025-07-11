using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public GameObject talkPanel;
    public bool isAction;

    public void Action(GameObject scanObj)
    {
        if (isAction)
        {
            isAction = false;
            //talkPanel.SetActive(false);
        }
        else
        {
            isAction = true;
            //talkPanel.SetActive(true);
            scanObject = scanObj;
            talkText.text = "this name is" + scanObject.name + "unamsay\n";
        }

        talkPanel.SetActive(isAction);

    }
}
