using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IdleEnemy : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Coroutine currentCoroutine;
    Transform targetPos;

    public GameObject bullet;
    public GameObject target;

    public int directionOfGaze = 0;         // 현재 바라보고 있는 방향
    public int fieldOfView = 0;             // 시야 범위

    public float shotDelay;                 // 공격 딜레이

    [Header("Stats")]
    public bool isFindPlayer = false;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;

    }


    void Update()
    {
        otherTasks();
        attack();

    }

    void otherTasks()
    {
        isFindPlayer = Vector2.Distance(targetPos.position, transform.position) < fieldOfView;

        if(isFindPlayer)
        {
            directionOfGaze = targetPos.position.x < transform.position.x ? -1 : 1;
            spriteRenderer.flipX = (directionOfGaze > 0);

        }
    }





    void attack()
    {
        if(isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("shot");

        }
    }

    IEnumerator shot()
    {
        Vector2 bulletPosision = new Vector2(transform.position.x + directionOfGaze / 1.5f, transform.position.y);
        GameObject instantBullet = Instantiate(bullet, bulletPosision, new Quaternion());
        EnemyBullet bulletScript = instantBullet.GetComponent<EnemyBullet>();

        bulletScript.setType(EnemyBullet.Type.Idle);
        float bulletSpeed = bulletScript.bulletSpeed;

        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity =
            new Vector2(
                (targetPos.position - transform.position).normalized.x * bulletSpeed,
                (targetPos.position - transform.position).normalized.y * bulletSpeed);

        yield return new WaitForSeconds(shotDelay);
        currentCoroutine = null;

    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }

    }
}


















































