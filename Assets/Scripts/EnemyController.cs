using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Transform texture;

    private bool facingRight = true;
    private int turned = 1;

    private Rigidbody2D rb;
    
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(turned * speed, rb.velocity.y);
    }

    private void Flip(int k) {
        Quaternion rot = transform.rotation;
        rot.y = k;
        transform.rotation = rot;
    }

    private void OnTriggerEnter2D(Collider2D other) {
    if (other.tag == "Turn") {
        if (facingRight) {
            Vector3 rotate = transform.eulerAngles;
            rotate.y = -180;
            texture.transform.rotation = Quaternion.Euler(rotate);
        } else {
            Vector3 rotate = transform.eulerAngles;
            rotate.y = 0;
            texture.transform.rotation = Quaternion.Euler(rotate);
        }

        facingRight = !facingRight;
        turned *= -1;
    }
}
}
