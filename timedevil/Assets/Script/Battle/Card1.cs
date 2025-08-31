using UnityEngine;

public class Card1 : MonoBehaviour, ICardPattern
{
    [SerializeField] string spritePath = "my_asset/Card1";
    // ������ �Ʒ� ���� (4��), �� ���� �ޡ������ 4����
    [SerializeField] string[] rows = new string[] { "1111", "0100", "1010", "0000" };

    public string SpritePath => spritePath;
    public string[] PatternRows => rows;
}
