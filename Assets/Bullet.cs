using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rb;
    public int damage = 1;
    public Vector2 direction;

    private float time = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeHit(damage);
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Ground")
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.5f);
        if (gameObject.activeInHierarchy)
        {
            time += Time.deltaTime;
            if (time >= 2)
            {
                gameObject.SetActive(false);
                time = 0;
            }
        }
    }
}
