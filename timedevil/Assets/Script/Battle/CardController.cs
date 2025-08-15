using UnityEngine;

public class CardController : MonoBehaviour
{
    [Header("Board & Visual")]
    public Transform enemyBoardOrigin;   // 적 4x4 보드 기준(좌하단 등)
    public float tileSize = 1f;          // 타일 크기
    public GameObject redMarkerPrefab;   // 빨간 표시 프리팹

    [Header("Battle")]
    public EnemyController enemy;        // 적 HP 스크립트
    public int damage = 10;              // 카드 데미지

    // 5 카드 패턴: 1=공격칸
    private readonly string[] patternRows = new string[4] {
        "1010",
        "0100",
        "1010",
        "0000"
    };

    private GameObject[] spawnedMarkers;
    private bool isActing = false;       // 연속 클릭 방지

    public void OnCardButton()
    {
        if (isActing) return;
        if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

        isActing = true;
        Debug.Log("🎴 [Card] 사용");

        ShowPattern();
        ApplyDamage();

        // 필요하면 연출 딜레이 넣기
        // StartCoroutine(EndAfterDelay(0.2f));
        EndNow();
    }

    void ShowPattern()
    {
        if (redMarkerPrefab == null || enemyBoardOrigin == null)
        {
            Debug.LogWarning("[CardController] redMarkerPrefab/enemyBoardOrigin 미지정");
            return;
        }

        spawnedMarkers = new GameObject[16];
        int idx = 0;

        for (int y = 0; y < 4; y++)
        {
            string row = patternRows[y];
            for (int x = 0; x < 4; x++)
            {
                if (row[x] == '1')
                {
                    Vector3 pos = enemyBoardOrigin.position + new Vector3(x * tileSize, y * tileSize, 0);
                    spawnedMarkers[idx] = Instantiate(redMarkerPrefab, pos, Quaternion.identity);
                }
                idx++;
            }
        }
    }

    void ClearPattern()
    {
        if (spawnedMarkers == null) return;
        foreach (var go in spawnedMarkers)
        {
            if (go != null) Destroy(go);
        }
        spawnedMarkers = null;
    }

    void ApplyDamage()
    {
        if (enemy == null)
        {
            Debug.LogWarning("[CardController] enemy 미지정");
            return;
        }
        enemy.TakeDamage(damage);
        Debug.Log($"🎯 적에게 {damage} 데미지");
    }

    void EndNow()
    {
        ClearPattern();               // 마커 제거
        isActing = false;
        TurnManager.Instance.EndPlayerTurn();   // 턴 진행
        // 버튼 on/off는 TurnManager가 처리함 (여기서 건드리지 않음)
    }

    //IEnumerator EndAfterDelay(float t) { yield return new WaitForSeconds(t); EndNow(); }
}
