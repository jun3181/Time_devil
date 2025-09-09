using UnityEngine;

public class Card1 : MonoBehaviour, ICardPattern
{
    [Header("ī�� �̹��� (Resources ���)")]
    [SerializeField] private string cardImagePath = "my_asset/Card1";

    // ���� ����: 1111 / 0000 / 1111 / 0000  => �¡��, ����Ʒ��� 16����
    [Header("���� (���� 16, '0'/'1'��)")]
    [SerializeField] private string pattern16 = "1111000011110000";

    public string CardImagePath => cardImagePath;
    public string Pattern16 => pattern16;
}
