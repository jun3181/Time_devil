using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class RunController : MonoBehaviour
{
    [SerializeField] private Button runBtn;
    [SerializeField] private AttackController attackController; // 있으면 마커 정리
    [SerializeField] private string myroomSceneName = "Myroom";
    [SerializeField] private float exitDelay = 0f; // 필요 없으면 0

    void Awake()
    {
        if (runBtn) runBtn.onClick.AddListener(TryRun);
    }

    public void TryRun()
    {
        // 100% 성공
        if (attackController) attackController.ClearImmediate();
        if (TurnManager.Instance) TurnManager.Instance.CancelInvoke();

        Time.timeScale = 1f;

        if (exitDelay > 0f) StartCoroutine(ExitAfterDelay());
        else ExitNow();
    }

    IEnumerator ExitAfterDelay()
    {
        yield return new WaitForSeconds(exitDelay);
        ExitNow();
    }

    void ExitNow()
    {
        SceneManager.LoadScene(myroomSceneName, LoadSceneMode.Single);
    }
}
