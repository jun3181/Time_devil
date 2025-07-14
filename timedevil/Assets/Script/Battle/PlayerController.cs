using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int currentPos = new Vector2Int(0, 0); // 현재 좌표

    private float tileSize = 1.0f; // 타일 한 칸의 크기

    // 현재 좌표를 갱신하면서 이동
    public void SetPosition(Vector2Int pos)
    {
        currentPos = pos;
        transform.position = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
    }
}
