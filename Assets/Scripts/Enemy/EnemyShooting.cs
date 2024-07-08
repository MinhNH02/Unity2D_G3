using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab của đạn
    public float bulletSpeed = 5f;   // Tốc độ của đạn
    public int numberOfBullets = 12; // Số lượng đạn
    public float shootInterval = 2f; // Thời gian giữa các lần bắn
    public float startDelay = 0f;    // Thời gian chờ trước khi bắt đầu bắn

    private void Start()
    {
        // Bắt đầu coroutine bắn đạn
        //StartCoroutine(ShootBullets());

        // Bắt đầu việc bắn đạn sau một khoảng thời gian delay

        startDelay += Time.deltaTime;
        if (startDelay >= 2)
        {
            StartCoroutine(ShootBullets());
            startDelay = 0f;
        }
    }

    IEnumerator ShootBullets()
    {
        while (true)
        {
            // Bắn đạn theo hình tròn
            ShootInCircle();

            // Chờ shootInterval giây trước khi bắn tiếp
            yield return new WaitForSeconds(shootInterval);
        }
    }

    void ShootInCircle()
    {
        // Tính toán góc giữa các viên đạn
        float angleStep = 360f / numberOfBullets;
        float angle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            // Tính toán vị trí của đạn
            float bulletDirXPosition = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180) * 0.5f;
            float bulletDirYPosition = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180) * 0.5f;

            Vector3 bulletVector = new Vector3(bulletDirXPosition, bulletDirYPosition, 0);
            Vector3 bulletMoveDirection = (bulletVector - transform.position).normalized * bulletSpeed;

            // Tạo đạn tại vị trí đã tính toán
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y);

            // Tăng góc để tính toán vị trí cho viên đạn tiếp theo
            angle += angleStep;
        }
    }
}
