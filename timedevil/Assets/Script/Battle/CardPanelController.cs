using UnityEngine;
using UnityEngine.UI;

public class CardPanelController : MonoBehaviour
{
    [Header("UI")]
    public GameObject cardGroup;     // Canvas 하위의 card_group (처음엔 비활성화)
    public Button card5Button;       // card_group 안의 5.png 버튼
    public Image card5Image;         // 5.png를 표시할 Image

    [Header("Resource")]
    public string card5SpritePath = "my_asset/5"; // Assets/Resources/my_asset/5.png

    bool isOpen = false;

    // 하단 "Card" 버튼에서 호출
    public void OnCardButton()
    {
        if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

        // 5.png 로드 후 패널 열기
        var sp = Resources.Load<Sprite>(card5SpritePath);
        if (sp == null)
        {
            Debug.LogWarning($"[CardPanel] 스프라이트를 찾을 수 없음: {card5SpritePath}");
            return;
        }

        card5Image.sprite = sp;

        // 중복 등록 방지하고 클릭 핸들러 연결
        card5Button.onClick.RemoveListener(OnCard5Clicked);
        card5Button.onClick.AddListener(OnCard5Clicked);

        cardGroup.SetActive(true);
        isOpen = true;

        Debug.Log("[CardPanel] 카드 선택 패널 열림 (5.png 표시)");
    }

    // 카드(5.png) 클릭 시
    public void OnCard5Clicked()
    {
        if (!isOpen) return;

        Debug.Log("[CardPanel] 5 클릭됨 → 공격패턴 구현 전 단계: 턴 종료");

        // 패널 닫기
        cardGroup.SetActive(false);
        isOpen = false;

        // 적 턴으로 전환
        TurnManager.Instance.EndPlayerTurn();

        // (다음 단계에서 여기서 공격패턴 표시 / 데미지 적용을 추가)
        // ShowAttackPattern(); enemy.TakeDamage(...);
    }
}
