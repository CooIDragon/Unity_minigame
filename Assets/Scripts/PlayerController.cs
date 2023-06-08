using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isDead = false;

    public float speed;
    public float jumpForce;
    public float moveInputX;

    public Transform frontCheck;

    private bool hasEntered;
    private bool facingRight = true;
    private bool inAir;
    private bool isWallFront = false;
    private bool extraJumps = true;

    private Rigidbody2D rb;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(frontCheck.position, 0.1f);
        isWallFront = colliders.Length > 0;

        if (transform.position.y < -7) {
            isDead = true;
        }

        if (isDead && !hasEntered) {
            GameManager.instance.Restart();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isWallFront && extraJumps) {
            doJump();
            extraJumps = false;
        }  else if (Input.GetKeyDown(KeyCode.Space) && !inAir) {
            doJump();
            inAir = true;
        }
    }

    private void FixedUpdate() {
        moveInputX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(moveInputX * speed, rb.velocity.y);

        if (facingRight == false && moveInputX > 0) {
            Flip();
        } else if (facingRight == true && moveInputX < 0) {
            Flip();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Enemy") {
            isDead = true;
        }
    }

    private void Flip() {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground") || collision.gameObject.tag.Equals("Wall"))
            inAir = false;
            extraJumps = true;
    }

    private void doJump() {
        rb.velocity = Vector2.up * jumpForce;
    }
}
