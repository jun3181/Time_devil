using UnityEngine;

public enum TurnState
{
    PlayerTurn,
    EnemyTurn
}

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    public TurnState currentTurn;

    void Awake()
    {
        // 싱글톤 패턴
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        currentTurn = TurnState.PlayerTurn;
        Debug.Log("🔷 플레이어 턴 시작");

        EnablePlayerButtons(true);
    }

    public void EndPlayerTurn()
    {
        Debug.Log("🔷 플레이어 턴 종료");

        EnablePlayerButtons(false);
        StartEnemyTurn();
    }

    public void StartEnemyTurn()
    {
        currentTurn = TurnState.EnemyTurn;
        Debug.Log("🔷 적 턴 시작");

        // 적 행동 실행
        EnemyAction();
    }

    public void EndEnemyTurn()
    {
        Debug.Log("🔷 적 턴 종료");

        StartPlayerTurn();
    }

    void EnemyAction()
    {
        // TODO: 여기서 적의 패턴을 구현
        Debug.Log("적이 공격합니다!");
        Invoke(nameof(EndEnemyTurn), 1.0f); // 1초 뒤 턴 종료
    }

    void EnablePlayerButtons(bool enable)
    {
        GameObject.Find("Card").GetComponent<UnityEngine.UI.Button>().interactable = enable;
        GameObject.Find("Move").GetComponent<UnityEngine.UI.Button>().interactable = enable;
        GameObject.Find("Item").GetComponent<UnityEngine.UI.Button>().interactable = enable;
        GameObject.Find("Run").GetComponent<UnityEngine.UI.Button>().interactable = enable;
    }
}
