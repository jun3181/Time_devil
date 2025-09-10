using UnityEngine;

public class Card1 : MonoBehaviour, ICardPattern
{
    [SerializeField] private string cardImagePath = "my_asset/Card1";
    [SerializeField] private string pattern16 = "1111000011110000";

    public string CardImagePath => cardImagePath;
    public string Pattern16 => pattern16;
}
