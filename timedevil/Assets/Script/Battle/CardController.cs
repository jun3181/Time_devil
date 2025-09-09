using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button openCardButton;  // "Card" 버튼
    [SerializeField] private GameObject cardGroup;   // 패널
    [SerializeField] private Image cardImage;        // 카드 보여줄 Image
    [SerializeField] private Button cardImageButton; // 이 Image에 붙은 Button(클릭용)

    [Header("Refs")]
    [SerializeField] private MonoBehaviour patternProvider; // ICardPattern 구현체 (예: Image에 붙은 Card1)
    [SerializeField] private AttackController attackController;

    private ICardPattern _pattern;

    private void Awake()
    {
        // 1) 버튼 리스너 먼저 연결 (에러가 있어도 Card 패널은 열리게)
        if (openCardButton != null) openCardButton.onClick.AddListener(OpenPanel);
        if (cardImageButton != null) cardImageButton.onClick.AddListener(OnCardClicked);

        if (cardGroup != null) cardGroup.SetActive(false);

        // 2) Pattern Provider 검사 (여기서 return 하지 말고, 경고만) 
        if (patternProvider == null)
        {
            Debug.LogError("[CardController] Pattern Provider가 비어있음 (ICardPattern 구현 컴포넌트를 드래그하세요)");
            return; // ← 패널만 열리면 된다면 이 return 지워도 됨. 눌렀을 때만 막으면 됨.
        }

        _pattern = patternProvider as ICardPattern;
        if (_pattern == null)
        {
            Debug.LogError("[CardController] Pattern Provider가 ICardPattern을 구현하지 않음");
            return; // ← 마찬가지로 필요 시 경고만 하고 진행해도 됨
        }

        // 3) 카드 이미지 로드
        if (cardImage != null)
        {
            var sprite = Resources.Load<Sprite>(_pattern.CardImagePath);
            if (sprite == null)
            {
                Debug.LogError($"[CardController] 카드 스프라이트를 찾을 수 없음: {_pattern.CardImagePath}");
            }
            else
            {
                cardImage.sprite = sprite;
                cardImage.preserveAspect = true;
                cardImage.raycastTarget = true;
            }
        }
    }


    private void OpenPanel()
    {
        if (cardGroup != null) cardGroup.SetActive(true);
    }

    private void OnCardClicked()
    {
        if (_pattern == null || attackController == null)
        {
            Debug.LogError("[CardController] _pattern 또는 attackController가 없음");
            return;
        }

        // 패턴 실행
        attackController.ShowPattern(_pattern.Pattern16);

        // 패널 닫기
        if (cardGroup != null) cardGroup.SetActive(false);
    }
}
