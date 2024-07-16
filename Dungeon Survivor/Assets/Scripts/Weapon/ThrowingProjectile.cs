using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowingProjectile : MonoBehaviour
{
    public float attackArea = 0.7f;
    Vector3 direction;
    [SerializeField] float speed;
    public int dmg = 5;
    public int numOfHits = 1;

    List<IDamageable> enemiesHit;

    float timeToDelete = 2f;

    public void setDirection(float dir_x, float dir_y)
    {
        direction = new Vector3(dir_x, dir_y);
        if (dir_x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = scale.x * -1;
            transform.localScale = scale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Time.frameCount % 6 == 0)
        {
            HitDetection();
        }

        TimerToLive();
    }

    private void TimerToLive()
    {
        timeToDelete -= Time.deltaTime;
        if (timeToDelete < 0)
        {
            Destroy(gameObject);
        }
    }

    private void HitDetection()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position, attackArea);
        foreach (Collider2D c in hit)
        {
            if (numOfHits > 0)
            {
                IDamageable enemy = c.GetComponent<IDamageable>();
                if (enemy != null)
                {
                    if (CheckRepeatHit(enemy) == false)
                    {
                        PostDamage(dmg, transform.position);
                        enemiesHit.Add(enemy);
                        enemy.TakeDamage(dmg);
                        numOfHits -= 1;
                    }
                }
            }
            else { break; }
        }
        if (numOfHits <= 0)
        {
            Destroy(gameObject);
        }
    }

    private bool CheckRepeatHit(IDamageable enemy)
    {
        if (enemiesHit == null)
        {
            enemiesHit = new List<IDamageable>();
        }
        return enemiesHit.Contains(enemy);
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void PostDamage(int damage, Vector3 worldPosition)
    {
        MessageSystem.instance.PostMessage(damage.ToString(), worldPosition);
    }
}
