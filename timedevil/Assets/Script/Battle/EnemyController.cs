using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Status")]
    public int maxHp = 50;
    [HideInInspector] public int hp;

    void Awake()
    {
        hp = maxHp;
    }

    public void TakeDamage(int dmg)
    {
        if (dmg < 0) dmg = 0;
        hp = Mathf.Max(0, hp - dmg);
        Debug.Log($"💥 Enemy HP: {hp}/{maxHp}");

        if (hp <= 0)
        {
            OnDefeated();
        }
    }

    void OnDefeated()
    {
        Debug.Log("✅ 적 처치, 전투 승리했습니다.");

        // 전투 종료 처리 필요 시 여기서 진행
        // (예: 버튼 비활성화, 씬 전환 등)
        // TurnManager 호출이 필요하면 아래처럼 사용 가능:
        // TurnManager.Instance.enabled = false;
        // or SceneManager.LoadScene("Mainmenu");
    }
}
