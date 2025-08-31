using UnityEngine;

public class Card1 : MonoBehaviour, ICardPattern
{
    [SerializeField] string spritePath = "my_asset/Card1";
    // 위에서 아래 순서 (4줄), 각 줄은 왼→오른쪽 4글자
    [SerializeField] string[] rows = new string[] { "1111", "0100", "1010", "0000" };

    public string SpritePath => spritePath;
    public string[] PatternRows => rows;
}
