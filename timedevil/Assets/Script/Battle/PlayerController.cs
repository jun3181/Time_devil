using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2Int currentPos = new Vector2Int(0, 0); // ���� ��ǥ

    private float tileSize = 1.0f; // Ÿ�� �� ĭ�� ũ��

    // ���� ��ǥ�� �����ϸ鼭 �̵�
    public void SetPosition(Vector2Int pos)
    {
        currentPos = pos;
        transform.position = new Vector3(pos.x * tileSize, pos.y * tileSize, 0);
    }
}
