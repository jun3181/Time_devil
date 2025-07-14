using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public void LoadBattleScene(GameObject scanObj)
    {
        string sceneName = "battle";

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
            return;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
