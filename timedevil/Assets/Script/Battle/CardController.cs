using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("UI")]
    public Button openCardButton;     // "Card" ��ư
    public GameObject cardGroup;      // �г� (card_group)
    public Button cardImageButton;    // �г� ��, �̹����� Ŭ������ ��ư
    public Image cardImage;           // �г� ��, ǥ�õ� ī�� �̹���

    [Header("Refs")]
    public AttackController attackController; // AttackController ����
    public MonoBehaviour cardPatternProvider; // ICardPattern ����(��: Card1)

    ICardPattern _pattern;

    void Awake()
    {
        // �⺻ ����
        if (!openCardButton || !cardGroup || !cardImageButton || !cardImage)
            Debug.LogError("[CardController] UI ���� ����!");

        if (!attackController) Debug.LogError("[CardController] AttackController ���� ����!");
        if (!cardPatternProvider) Debug.LogError("[CardController] CardPatternProvider ���� ����!");

        _pattern = cardPatternProvider as ICardPattern;
        if (_pattern == null) Debug.LogError("[CardController] provider�� ICardPattern�� �ƴ�!");
    }

    void Start()
    {
        // ���� �� �г� ����
        cardGroup.SetActive(false);

        // Card ��ư : �г� ���
        openCardButton.onClick.RemoveAllListeners();
        openCardButton.onClick.AddListener(() =>
        {
            // �÷��̾� �Ͽ��� ����
            if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

            // �̹��� ����
            TryLoadCardSprite();
            cardGroup.SetActive(true);
        });

        // �̹��� Ŭ��(=ī�� ���)
        cardImageButton.onClick.RemoveAllListeners();
        cardImageButton.onClick.AddListener(OnUseCard);
    }

    void TryLoadCardSprite()
    {
        if (_pattern == null) return;

        var sp = Resources.Load<Sprite>(_pattern.SpritePath);
        if (!sp)
        {
            Debug.LogWarning($"[CardController] ��������Ʈ �ε� ����: {_pattern.SpritePath}");
            return;
        }
        cardImage.sprite = sp;
        cardImage.preserveAspect = true;
        cardImage.raycastTarget = true; // Ŭ�� �޵���
    }

    void OnUseCard()
    {
        if (_pattern == null) return;

        // ī�� ��� �� ���� ǥ�� �ڷ�ƾ
        attackController.ShowPattern(_pattern.PatternRows);

        // �г� �ݱ�
        cardGroup.SetActive(false);
    }
}
