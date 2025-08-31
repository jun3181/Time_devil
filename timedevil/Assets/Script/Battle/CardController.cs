using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("UI")]
    public Button openCardButton;     // "Card" 버튼
    public GameObject cardGroup;      // 패널 (card_group)
    public Button cardImageButton;    // 패널 안, 이미지를 클릭받을 버튼
    public Image cardImage;           // 패널 안, 표시될 카드 이미지

    [Header("Refs")]
    public AttackController attackController; // AttackController 참조
    public MonoBehaviour cardPatternProvider; // ICardPattern 구현(예: Card1)

    ICardPattern _pattern;

    void Awake()
    {
        // 기본 검증
        if (!openCardButton || !cardGroup || !cardImageButton || !cardImage)
            Debug.LogError("[CardController] UI 참조 빠짐!");

        if (!attackController) Debug.LogError("[CardController] AttackController 참조 없음!");
        if (!cardPatternProvider) Debug.LogError("[CardController] CardPatternProvider 참조 없음!");

        _pattern = cardPatternProvider as ICardPattern;
        if (_pattern == null) Debug.LogError("[CardController] provider가 ICardPattern이 아님!");
    }

    void Start()
    {
        // 시작 시 패널 끄기
        cardGroup.SetActive(false);

        // Card 버튼 : 패널 토글
        openCardButton.onClick.RemoveAllListeners();
        openCardButton.onClick.AddListener(() =>
        {
            // 플레이어 턴에만 열기
            if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

            // 이미지 세팅
            TryLoadCardSprite();
            cardGroup.SetActive(true);
        });

        // 이미지 클릭(=카드 사용)
        cardImageButton.onClick.RemoveAllListeners();
        cardImageButton.onClick.AddListener(OnUseCard);
    }

    void TryLoadCardSprite()
    {
        if (_pattern == null) return;

        var sp = Resources.Load<Sprite>(_pattern.SpritePath);
        if (!sp)
        {
            Debug.LogWarning($"[CardController] 스프라이트 로드 실패: {_pattern.SpritePath}");
            return;
        }
        cardImage.sprite = sp;
        cardImage.preserveAspect = true;
        cardImage.raycastTarget = true; // 클릭 받도록
    }

    void OnUseCard()
    {
        if (_pattern == null) return;

        // 카드 사용 → 공격 표시 코루틴
        attackController.ShowPattern(_pattern.PatternRows);

        // 패널 닫기
        cardGroup.SetActive(false);
    }
}
