using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float jumpForce = 5f; // Lực nhảy
    private bool isGrounded; // Kiểm tra nếu nhân vật đang trên mặt đất
    private Rigidbody2D rb; // Tham chiếu đến thành phần Rigidbody2D
    private bool facingRight = true; // Kiểm tra hướng hiện tại của nhân vật
    private Animator animator; // Tham chiếu đến thành phần Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy thành phần Rigidbody2D
        animator = GetComponent<Animator>(); // Lấy thành phần Animator
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from this game object");
        }
        if (animator == null)
        {
            Debug.LogError("Animator component is missing from this game object");
        }
    }

    void Update()
    {
        // Di chuyển trái và phải
        float move = Input.GetAxis("Horizontal") * moveSpeed;
        rb.velocity = new Vector2(move, rb.velocity.y);

        // Kiểm tra và lật mặt nhân vật
        if (move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }

        // Cập nhật tham số Speed cho Animator
        animator.SetFloat("run", Mathf.Abs(move));

        // Nhảy
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void Flip()
    {
        // Lật mặt nhân vật
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu nhân vật đang chạm đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            // Cập nhật tham số cho Animator
            animator.SetBool("isGrounded", true);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Khi nhân vật không còn chạm đất
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            // Cập nhật tham số cho Animator
            animator.SetBool("isGrounded", false);
        }
    }
}
