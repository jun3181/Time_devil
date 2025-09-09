using UnityEngine;

public class Card1 : MonoBehaviour, ICardPattern
{
    [Header("카드 이미지 (Resources 경로)")]
    [SerializeField] private string cardImagePath = "my_asset/Card1";

    // 예시 패턴: 1111 / 0000 / 1111 / 0000  => 좌→우, 위→아래로 16글자
    [Header("패턴 (길이 16, '0'/'1'만)")]
    [SerializeField] private string pattern16 = "1111000011110000";

    public string CardImagePath => cardImagePath;
    public string Pattern16 => pattern16;
}
