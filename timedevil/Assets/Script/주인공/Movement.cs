using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator animator;

    private Vector2 lastMoveDir = Vector2.down; // 초기 방향

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement = Vector2.zero;

        if (Input.GetKey(KeyCode.UpArrow))
            movement.y = 1;
        else if (Input.GetKey(KeyCode.DownArrow))
            movement.y = -1;

        if (Input.GetKey(KeyCode.RightArrow))
            movement.x = 1;
        else if (Input.GetKey(KeyCode.LeftArrow))
            movement.x = -1;

        movement.Normalize();

        bool isWalking = movement != Vector2.zero;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            // 가장 큰 축으로 방향 결정
            if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
            {
                lastMoveDir.x = movement.x > 0 ? 1 : -1;
                lastMoveDir.y = 0;
            }
            else
            {
                lastMoveDir.x = 0;
                lastMoveDir.y = movement.y > 0 ? 1 : -1;
            }
        }

        animator.SetFloat("MoveX", lastMoveDir.x);
        animator.SetFloat("MoveY", lastMoveDir.y);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
