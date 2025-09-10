using UnityEngine;

public class Enemy1 : MonoBehaviour, ICardPattern
{
    [SerializeField] string cardImagePath = "my_asset/Enemy1"; // 안 써도 OK
    [SerializeField] string pattern16 = "1111111111111111";

    public string CardImagePath => cardImagePath;
    public string Pattern16 => pattern16;
}
