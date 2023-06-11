using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;

    public Transform frontCheck;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float moveInputX;

    private bool hasEntered;
    private bool isDead;
    private bool inAir;
    private bool isWallFront;
    private bool extraJumps;
    private bool facingRight = true;

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

        if (Input.GetKeyDown(KeyCode.Space) && isWallFront && extraJumps && inAir) {
            doJump(10);
            extraJumps = false;
        }  else if (Input.GetKeyDown(KeyCode.Space) && !inAir) {
            doJump(8);
            inAir = true;
        }
    }

    private void FixedUpdate() {
        inAir = !Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround)){
            extraJumps = true;
        }

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

    private void doJump(int k) {
        rb.velocity = Vector2.up * jumpForce * k / 8;
    }
}
