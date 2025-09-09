using UnityEngine;
using UnityEngine.UI;

public class CardController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button openCardButton;  // "Card" ��ư
    [SerializeField] private GameObject cardGroup;   // �г�
    [SerializeField] private Image cardImage;        // ī�� ������ Image
    [SerializeField] private Button cardImageButton; // �� Image�� ���� Button(Ŭ����)

    [Header("Refs")]
    [SerializeField] private MonoBehaviour patternProvider; // ICardPattern ����ü (��: Image�� ���� Card1)
    [SerializeField] private AttackController attackController;

    private ICardPattern _pattern;

    private void Awake()
    {
        // 1) ��ư ������ ���� ���� (������ �־ Card �г��� ������)
        if (openCardButton != null) openCardButton.onClick.AddListener(OpenPanel);
        if (cardImageButton != null) cardImageButton.onClick.AddListener(OnCardClicked);

        if (cardGroup != null) cardGroup.SetActive(false);

        // 2) Pattern Provider �˻� (���⼭ return ���� ����, ���) 
        if (patternProvider == null)
        {
            Debug.LogError("[CardController] Pattern Provider�� ������� (ICardPattern ���� ������Ʈ�� �巡���ϼ���)");
            return; // �� �гθ� ������ �ȴٸ� �� return ������ ��. ������ ���� ������ ��.
        }

        _pattern = patternProvider as ICardPattern;
        if (_pattern == null)
        {
            Debug.LogError("[CardController] Pattern Provider�� ICardPattern�� �������� ����");
            return; // �� ���������� �ʿ� �� ��� �ϰ� �����ص� ��
        }

        // 3) ī�� �̹��� �ε�
        if (cardImage != null)
        {
            var sprite = Resources.Load<Sprite>(_pattern.CardImagePath);
            if (sprite == null)
            {
                Debug.LogError($"[CardController] ī�� ��������Ʈ�� ã�� �� ����: {_pattern.CardImagePath}");
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
            Debug.LogError("[CardController] _pattern �Ǵ� attackController�� ����");
            return;
        }

        // ���� ����
        attackController.ShowPattern(_pattern.Pattern16);

        // �г� �ݱ�
        if (cardGroup != null) cardGroup.SetActive(false);
    }
}
