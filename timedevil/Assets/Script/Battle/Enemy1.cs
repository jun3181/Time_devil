using UnityEngine;

public class Enemy1 : MonoBehaviour, ICardPattern
{
    [SerializeField] private string cardImagePath = "my_asset/Enemy1";
    [SerializeField] private string pattern16 = "1111111111111111";

    // 16칸의 발동 시간 (예: 전부 0초 → 동시에 발동)
    [SerializeField] private float[] timings = new float[16]
    {
        1f, 2f, 3f, 4f,
        5f, 6f, 7f, 8f,
        9f, 10f, 11f, 12f,
        13f, 14f, 15f, 16f
    };

    public string CardImagePath => cardImagePath;
    public string Pattern16 => pattern16;
    public float[] Timings => timings;
}
