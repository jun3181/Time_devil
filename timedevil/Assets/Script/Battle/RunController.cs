using UnityEngine;
using UnityEngine.SceneManagement;

public class RunController : MonoBehaviour
{
    [SerializeField] private AttackController attackController;

    public void OnRun()
    {
        // 연출 클리어
        if (attackController) attackController.ClearAllImmediate();

        // 도망 100% 가정 → Myroom 복귀
        SceneManager.LoadScene("Myroom");
    }
}
