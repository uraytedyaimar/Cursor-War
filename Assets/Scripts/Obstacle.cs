using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField] private float moveSpeed;
    private float randomRotation;
    private Vector3 currentRandomMovement;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        currentRandomMovement = GetRandomMovement();
        randomRotation = Random.Range(-180, 180);
    }

    private void Update() {
        Vector3 moveDir = (currentRandomMovement - transform.position).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
        transform.Rotate(0, 0, randomRotation * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentRandomMovement) < 0.1f) {
            Destroy(gameObject);
        }
    }

    private Vector3 GetRandomMovement() {
        float randomX = Random.Range(-18, 18);
        float randomY = Random.Range(-10, 10);

        return new Vector2(randomX, randomY);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Mouse mouse = collision.GetComponent<Mouse>();
        if (mouse != null) {
            mouse.Damage(10);
        }
    }
}
