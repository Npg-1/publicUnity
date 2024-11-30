using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float hp;
    public float speed;

    float hAxis;
    float vAxis;

    void Awake()
    {

    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        Vector2 moveVecX = new Vector2(hAxis, 0).normalized;

        // 플레이어의 위치는 현재 위치 + moveVecX에서 speed와 걷고있는지에 따라서 결정됨
        transform.position = (new Vector2(transform.position.x, transform.position.y)) + moveVecX * speed * Time.deltaTime;

        Vector2 moveVecY = new Vector2(0, vAxis).normalized;

        // 플레이어의 위치는 현재 위치 + moveVecX에서 speed와 걷고있는지에 따라서 결정됨
        transform.position = (new Vector2(transform.position.x, transform.position.y)) + moveVecY * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, transform.position.y, -0.01f);


        if(hp < 0)
        {
            Destroy(gameObject);

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "EnemyBullet(Clone)")
        {
            EnemyBullet eb = collision.GetComponent<EnemyBullet>();
            hp -= eb.damage;


        }
    }

}
