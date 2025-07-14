using UnityEngine;

public class MoveController : MonoBehaviour
{
    public Transform playerTransform;

    private Vector2Int currentPos = new Vector2Int(0, 0); // 현재 좌표
    private float tileSize = 1.0f; // 타일 한 칸 크기

    private bool isMoving = false;

    public void StartMove()
    {
        isMoving = true;
    }

    void Update()
    {
        if (!isMoving) return;

        if (Input.GetKeyDown(KeyCode.UpArrow))
            TryMove(Vector2Int.up);
        else if (Input.GetKeyDown(KeyCode.DownArrow))
            TryMove(Vector2Int.down);
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
            TryMove(Vector2Int.left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            TryMove(Vector2Int.right);
    }

    void TryMove(Vector2Int dir)
    {
        Vector2Int nextPos = currentPos + dir;

        // 경계 체크
        if (nextPos.x < 0 || nextPos.x > 3 || nextPos.y < 0 || nextPos.y > 3)
            return;

        currentPos = nextPos;

        Vector3 worldPos = new Vector3(currentPos.x * tileSize, currentPos.y * tileSize, 0);
        playerTransform.position = worldPos;
    }

    public void EndMove()
    {
        isMoving = false;
    }
}
