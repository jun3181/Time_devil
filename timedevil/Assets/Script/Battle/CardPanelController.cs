using UnityEngine;
using UnityEngine.UI;

public class CardPanelController : MonoBehaviour
{
    [Header("UI")]
    public GameObject cardGroup;     // Canvas ������ card_group (ó���� ��Ȱ��ȭ)
    public Button card5Button;       // card_group ���� 5.png ��ư
    public Image card5Image;         // 5.png�� ǥ���� Image

    [Header("Resource")]
    public string card5SpritePath = "my_asset/5"; // Assets/Resources/my_asset/5.png

    bool isOpen = false;

    // �ϴ� "Card" ��ư���� ȣ��
    public void OnCardButton()
    {
        if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

        // 5.png �ε� �� �г� ����
        var sp = Resources.Load<Sprite>(card5SpritePath);
        if (sp == null)
        {
            Debug.LogWarning($"[CardPanel] ��������Ʈ�� ã�� �� ����: {card5SpritePath}");
            return;
        }

        card5Image.sprite = sp;

        // �ߺ� ��� �����ϰ� Ŭ�� �ڵ鷯 ����
        card5Button.onClick.RemoveListener(OnCard5Clicked);
        card5Button.onClick.AddListener(OnCard5Clicked);

        cardGroup.SetActive(true);
        isOpen = true;

        Debug.Log("[CardPanel] ī�� ���� �г� ���� (5.png ǥ��)");
    }

    // ī��(5.png) Ŭ�� ��
    public void OnCard5Clicked()
    {
        if (!isOpen) return;

        Debug.Log("[CardPanel] 5 Ŭ���� �� �������� ���� �� �ܰ�: �� ����");

        // �г� �ݱ�
        cardGroup.SetActive(false);
        isOpen = false;

        // �� ������ ��ȯ
        TurnManager.Instance.EndPlayerTurn();

        // (���� �ܰ迡�� ���⼭ �������� ǥ�� / ������ ������ �߰�)
        // ShowAttackPattern(); enemy.TakeDamage(...);
    }
}
