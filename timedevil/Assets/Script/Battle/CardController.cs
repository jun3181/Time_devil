// CardController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button openCardButton;   // "Card" 버튼
    [SerializeField] private GameObject cardGroup;    // 카드 패널
    [SerializeField] private Image cardImage;         // 카드 이미지
    [SerializeField] private Button cardImageButton;  // 카드 이미지 클릭용 버튼

    [Header("Refs")]
    [SerializeField] private AttackController attackController;

    [Header("Resources 폴더")]
    [SerializeField] private string resourcesFolder = "my_asset";

    // 이번 선택에서 사용할 카드 이름 (예: "Card1")
    private string currentCardName;

    void Awake()
    {
        if (openCardButton) openCardButton.onClick.AddListener(OpenCardPanel);
        if (cardImageButton) cardImageButton.onClick.AddListener(OnCardImageClicked);
        if (cardGroup) cardGroup.SetActive(false);
    }

    // ▶ Card 버튼
    private void OpenCardPanel()
    {
        if (!cardGroup) return;
        cardGroup.SetActive(true);

        // DB에서 첫 번째 CardX 선택
        currentCardName = FindFirstCardName(ItemDatabase.Instance?.collectedItems);

        // 이미지 로드
        if (cardImage)
        {
            if (string.IsNullOrEmpty(currentCardName))
            {
                cardImage.sprite = null;
                return;
            }

            var sprite = Resources.Load<Sprite>($"{resourcesFolder}/{currentCardName}");
            cardImage.sprite = sprite;
            cardImage.preserveAspect = true;
            cardImage.raycastTarget = true;
            cardImage.enabled = sprite != null;
        }
    }

    // ▶ 카드 이미지 클릭
    private void OnCardImageClicked()
    {
        if (string.IsNullOrEmpty(currentCardName) || attackController == null) return;

        // CardX라는 이름의 MonoBehaviour(=ICardPattern 구현)를 찾아 임시 GO에 붙이고 패턴을 읽는다.
        var t = FindTypeByName(currentCardName);
        if (t == null)
        {
            Debug.LogError($"[CardController] 타입을 찾을 수 없음: {currentCardName}");
            return;
        }

        var tmp = new GameObject($"_CardPattern_{currentCardName}");
        try
        {
            var comp = tmp.AddComponent(t);
            var pattern = comp as ICardPattern;
            if (pattern == null)
            {
                Debug.LogError($"[CardController] {currentCardName} 가 ICardPattern을 구현하지 않음");
                return;
            }

            attackController.ShowPattern(pattern.Pattern16); // 16칸 문자열 전달
        }
        finally
        {
            Destroy(tmp);
        }

        // 패널 닫기
        if (cardGroup) cardGroup.SetActive(false);
    }

    // 보유 목록에서 "Card"로 시작하는 첫 항목
    private string FindFirstCardName(List<string> items)
    {
        if (items == null) return null;
        foreach (var name in items)
            if (!string.IsNullOrEmpty(name) && name.StartsWith("Card"))
                return name.Trim();
        return null;
    }

    // 이름으로 타입 찾기 (어셈블리 전수검사)
    private static Type FindTypeByName(string typeName)
    {
        var asm = typeof(CardController).Assembly;
        return asm.GetTypes().FirstOrDefault(t => t.Name == typeName && typeof(MonoBehaviour).IsAssignableFrom(t));
    }
}
