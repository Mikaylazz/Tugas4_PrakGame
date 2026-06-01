using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("Referensi")]
    public Transform Player;

    [Header("Movement")]
    public float patrolspeed = 2f;
    public float chasespeed = 4f;

    [Header("Detection")]
    public float chaserange = 5f;
    public float attackrange = 1.2f;
    public float patrolchange = 3f;

    [Header("Damage")]
    public int damage = 1;

    [Header("Obstacle")]
    public LayerMask wallLayer;
    public float walldetect = 1f;

    private Rigidbody2D rb;
    private Vector2 move;
    private float patroltime;

    private EnemyState currentstate;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentstate = EnemyState.Patrol;

        ChooseRandom();

        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        float distance =
            Vector2.Distance(transform.position, Player.position);

        switch (currentstate)
        {
            case EnemyState.Patrol:
                Patrol(distance);
                break;

            case EnemyState.Chase:
                Chase(distance);
                break;

            case EnemyState.Attack:
                Attack(distance);
                break;
        }

        RotateEnemy();
    }

    void FixedUpdate()
    {
        float speed =
            currentstate == EnemyState.Chase
            ? chasespeed
            : patrolspeed;

        rb.velocity = move * speed;
    }

    void Patrol(float distance)
    {
        patroltime += Time.deltaTime;

        if (patroltime >= patrolchange)
        {
            ChooseRandom();
            patroltime = 0;
        }

        if (distance <= chaserange)
        {
            currentstate = EnemyState.Chase;
        }
    }

    void Chase(float distance)
    {
        move =
            (Player.position - transform.position).normalized;

        if (distance <= attackrange)
        {
            currentstate = EnemyState.Attack;
        }

        else if (distance >= chaserange)
        {
            currentstate = EnemyState.Patrol;
            ChooseRandom();
        }
    }

    void Attack(float distance)
    {
        move = Vector2.zero;

        Debug.Log("Enemy menyerang!");

        if (distance > attackrange)
        {
            currentstate = EnemyState.Chase;
        }
    }

    void ChooseRandom()
    {
        Vector2 direction;
        RaycastHit2D hit;

        do
        {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(-1f, 1f);

            direction =
                new Vector2(x, y).normalized;

            hit = Physics2D.Raycast(
                transform.position,
                direction,
                walldetect,
                wallLayer
            );

        } while (hit.collider != null);

        move = direction;
    }

    void RotateEnemy()
    {
        
        if (currentstate == EnemyState.Patrol)
        {
            if (move != Vector2.zero)
            {
                float angle =
                    Mathf.Atan2(move.y, move.x)
                    * Mathf.Rad2Deg;

                angle += 90f;

                transform.rotation =
                    Quaternion.RotateTowards(
                        transform.rotation,
                        Quaternion.Euler(0, 0, angle),
                        100f * Time.deltaTime
                    );
            }
        }

        else if (currentstate == EnemyState.Chase ||
                currentstate == EnemyState.Attack)
        {
            Vector2 direction =
                Player.position - transform.position;

            float angle =
                Mathf.Atan2(direction.y, direction.x)
                * Mathf.Rad2Deg;

            angle += 90f;

            transform.rotation =
                Quaternion.Euler(0, 0, angle);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        }

        if (currentstate == EnemyState.Patrol)
        {
            ChooseRandom();
        }
    }
}
