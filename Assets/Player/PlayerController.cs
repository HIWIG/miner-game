using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public TileClass selectedTile;

    public int playerRange;
    public float moveSpeed;
    public float jumpForce;
    public bool onGround;
    public float horizontal;

    private Rigidbody2D rb;
    private Animator animator;
    public bool hit;
    public bool place;

    public Vector2 spawPosition;
    public Vector2Int mousePosition;

    public TerrainGenerator terrainGenerator;

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

        hit = Input.GetMouseButton(0);
        place = Input.GetMouseButton(1);

        if(Vector2.Distance(transform.position, mousePosition) <= playerRange)
        {
            if (hit)
            {
                terrainGenerator.RemoveTile(mousePosition.x, mousePosition.y);
            }
            else if (place)
            {
                if (!terrainGenerator.worldTiles.Contains(new Vector2Int(mousePosition.x, mousePosition.y)) && mousePosition.x >= 0 && mousePosition.x <= terrainGenerator.worldSize && mousePosition.y >= 0 && mousePosition.y <= terrainGenerator.worldSize)
                    terrainGenerator.PlaceTile(selectedTile, mousePosition.x, mousePosition.y, true, 2);
            }
        }
        
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
        mousePosition.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x - 0.5f);
        mousePosition.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - 0.5f);

        animator.SetFloat("horizontal", horizontal);
        animator.SetBool("hit", hit || place);

        var position = GetComponent<Transform>().position;
        terrainGenerator.InsideCabin(position);
    }
}
