using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //public Slider healthbar;
    public float speed;
    private Animator animator;
    public float health = 10;

    public ObjectPooling bulletPool;

    public Vector3 spawn;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spawn = transform.position;
    }

    private void Update()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            dir.x = -1;
            animator.SetInteger("Direction", 3);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            dir.x = 1;
            animator.SetInteger("Direction", 2);
        }

        if (Input.GetKey(KeyCode.W))
        {
            dir.y = 1;
            animator.SetInteger("Direction", 1);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dir.y = -1;
            animator.SetInteger("Direction", 0);
        }

        dir.Normalize();
        animator.SetBool("IsMoving", dir.magnitude > 0);

        GetComponent<Rigidbody2D>().velocity = speed * dir;

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        //healthbar.value = health / 10;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("You die");
        transform.position = spawn;
        //healthbar.value = 1;
        health = 10;
    }

    public void Fire()
    {
        var bullet = bulletPool.GetPooledObject();
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.SetActive(true);

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 fireDirection = (mousePos - (Vector2)transform.position).normalized;
            bullet.GetComponent<Bullet>().direction = fireDirection;
            //bullet.GetComponent<Rigidbody2D>().velocity = fireDirection * 10;
        }
    }
}
