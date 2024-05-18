﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển
    public float jumpForce = 5f; // Lực nhảy
    public float dashDuration = 0.5f; // Thời gian lướt (đơn vị: giây)
    public float dashSpeedMultiplier = 10f; // Hệ số tốc độ Dash
    private bool isGrounded; // Kiểm tra nếu nhân vật đang trên mặt đất
    private Rigidbody2D rb; // Tham chiếu đến thành phần Rigidbody2D
    private bool facingRight = true; // Kiểm tra hướng hiện tại của nhân vật
    private Animator animator; // Tham chiếu đến thành phần Animator
    private bool isAttacking = false; // Kiểm tra nếu nhân vật đang tấn công
    private bool isDashing = false; // Kiểm tra nếu nhân vật đang Dash

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
        if (!isAttacking)
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
            animator.SetFloat("Speed", Mathf.Abs(move));

            // Nhảy
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
            }

            // Dash
            if (!isDashing && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(DashCoroutine());
            }
        }

        // Tấn công
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking)
        {
            StartCoroutine(Attack());
        }

        // Cập nhật trạng thái idle và run
        if (isGrounded)
        {
            animator.SetBool("isGrounded", true);
            if (rb.velocity.x == 0 && !isAttacking)
            {
                animator.SetBool("Idle", true);
                animator.SetBool("Run", false);
            }
            else
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Run", true);
            }
        }
        else
        {
            animator.SetBool("isGrounded", false);
            animator.SetBool("Run", false);
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

    private IEnumerator Attack()
    {
        isAttacking = true;
        rb.velocity = Vector2.zero; // Dừng chuyển động
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length); // Chờ thời gian của animation Attack
        isAttacking = false;
    }

    void Dash()
    {
        isDashing = true;
        // Thiết lập tốc độ di chuyển của nhân vật
        float dashSpeed = facingRight ? dashSpeedMultiplier : -dashSpeedMultiplier;
        rb.velocity = new Vector2(dashSpeed, rb.velocity.y);
        // Kích hoạt trạng thái lướt trong Animator
        animator.SetBool("IsDashing", true);
    }

    IEnumerator DashCoroutine()
    {
        isDashing = true;
        animator.SetBool("IsDashing", true); // Kích hoạt trạng thái lướt trong Animator
        yield return new WaitForSeconds(dashDuration); // Chờ cho đến khi kết thúc thời gian lướt
        animator.SetBool("IsDashing", false); // Kết thúc trạng thái lướt
        isDashing = false;
    }
}
