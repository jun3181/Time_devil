using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI talkText;
    public GameObject scanObject;
    public GameObject talkPanel;

    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        talkText.text = "this name is" + scanObject.name + "unamsay\n";

    }

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
