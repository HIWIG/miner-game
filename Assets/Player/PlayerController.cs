using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    public float horizontal;

    private Rigidbody2D rb;
    private Animator animator;

    public Vector2 spawPosition;

    public void Spawn()
    {
        GetComponent<Transform>().position = spawPosition;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            onGround = false;
        }
    }
    private void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");
        float vertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontal * moveSpeed, rb.velocity.y);

        if(horizontal > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if(horizontal < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if (vertical > 0.1f || jump > 0.1f)
        {
            if(onGround)
                movement.y = jumpForce;
        }
        rb.velocity = movement;
    }
    private void Update()
    {
        animator.SetFloat("horizontal", horizontal);
    }
}
