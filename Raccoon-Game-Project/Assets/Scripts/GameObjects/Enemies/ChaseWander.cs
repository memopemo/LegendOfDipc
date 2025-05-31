using UnityEngine;
using Animator2D;
using UnityEngine.Tilemaps;

public class ChaseWander : MonoBehaviour
{
    PlayerStateManager player;
    DirectionedObject direction;
    Rigidbody2D rigidbody2D;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<PlayerStateManager>();
        direction = GetComponent<DirectionedObject>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        RandomlyChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody2D.linearVelocity = direction.direction * 3;
        GetComponent<Animator2D.Animator2D>().SetAnimation(0);
    }
    void OnDisable()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
    }
    void RandomlyChangeDirection()
    {
        ChooseNewDirection();
        Invoke(nameof(RandomlyChangeDirection), Random.value * 3f + 1);
    }
    void ChooseNewDirection()
    {
        direction.direction = Vector2Int.RoundToInt((player.transform.position - transform.position).normalized);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player.gameObject) return;
        direction.direction *= -1;
    }
    public void OnTakeDamage()
    {
        GetComponent<Animator2D.Animator2D>().SetAnimation(1);
    }
}
