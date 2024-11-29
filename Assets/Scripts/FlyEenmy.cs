using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEenmy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Coroutine currentCoroutine;
    private Transform targetPos;


    [Header("Manage")]
    public bool stopPosition;

    [Space]
    public GameObject target;
    public GameObject bullet;
    public LayerMask groundLayer;

    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;
    public float bulletSpeed;
    public float shotDelay;
    public float pursuitMoveDelay;
    public float wanderMoveDelay;

    public Vector2 directionOfGaze = new Vector2();

    public int fieldOfView = 0;
    public int attackOfView = 0;


    [Space]
    [Header("State")]

    public bool isFindPlayer = false;
    public bool isAbleToAttack = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;

    }




    void Update()
    {
        boolCheck();

        move();

        manage();

    }


    void boolCheck()
    {
        isFindPlayer = Vector2.Distance(targetPos.position, transform.position) < fieldOfView;
        isAbleToAttack = Vector2.Distance(targetPos.position, transform.position) < attackOfView;

        spriteRenderer.flipX = (directionOfGaze.x > 0);

    }



    void move()
    {
        if (isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("pursuitPlayer");

        }
        else if (!isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("wander");

        }

        // 플레이어를 찾기 전: 그냥 돌아다니기
        // 플레이어 찾음 하지만 공격벜위는 아님: 플레이어가 공격범위로 들어올 때 까지 접근
        // 플레이어가 공격범위에 들어옴: 접근하다가 공격 이전에 잠시 정지했다가 공격함

        rigid.velocity = new Vector2(directionOfGaze.x * speed, directionOfGaze.y * speed);

    }

    IEnumerator pursuitPlayer()
    {
        while (true)
        {
            Debug.Log("pursuitPlayer()");
            directionOfGaze = new Vector2(targetPos.position.x - transform.position.x
                , targetPos.position.y - transform.position.y).normalized;
                //transform.position.x > targetPos.position.x ? -1 : 1;

            //StartCoroutine("shot");
            yield return new WaitForSeconds(pursuitMoveDelay);


            if (!isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }
        }
    }

    IEnumerator shot()
    {
        //Vector2 bulletPosision = new Vector2(transform.position.x + directionOfGaze / 1.5f, transform.position.y);
        //GameObject instantBullet = Instantiate(bullet, bulletPosision, new Quaternion());
        //Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        //bulletRigid.velocity =
        //    new Vector2(
        //        (targetPos.position - transform.position).normalized.x * bulletSpeed,
        //        (targetPos.position - transform.position).normalized.y * bulletSpeed);

        yield return null;

    }

    IEnumerator wander()
    {
        Debug.Log("wander()");
        while (true)
        {
            directionOfGaze = new Vector2(Random.Range(-10, 11), Random.Range(-10, 11)).normalized;

            yield return new WaitForSeconds(wanderMoveDelay);


            if (isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }

        }

    }


    // 임시로 잠시 붙여둔 거임! 삭제해도 됨!!
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }

    }




    void OnDrawGizmos() 
    {

    }


    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }














































}
