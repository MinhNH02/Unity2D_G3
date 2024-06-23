using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    private Rigidbody2D rb;
    public int unitToMove = 5;
    private Vector2 startPos;
    [SerializeField]
    private float health;
    //public EnemyHealthbar healthbarPrefab;
    //public EnemyHealthbar healthbar;
    public float Hitpoints;
    public float MaxHitpoints = 5;

    private GameObject player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        //healthbar = Instantiate(healthbarPrefab, GetComponentInParent<SpawnEnemyController>().enemyHealthbarTransform);
        //healthbar.Initialize(gameObject);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnEnable()
    {
        health = 3;
        Hitpoints = health;
        transform.localPosition = new Vector3(Random.RandomRange(2, -2), Random.RandomRange(-2, 2), 0);
        moveSpeed = Random.Range(50, 100);
        moveSpeed /= 100;
        //healthbar.gameObject.SetActive(true);
    }

    private void Update()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        // Move the enemy towards the player
        transform.Translate(moveSpeed * Time.deltaTime * direction);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void TakeHit(float damage)
    {
        Hitpoints -= damage;
        //healthbar.SetHealth(Hitpoints, MaxHitpoints);
        if (Hitpoints <= 0)
        {
            gameObject.SetActive(false);
            //healthbar.gameObject.SetActive(false);
        }
    }
}
