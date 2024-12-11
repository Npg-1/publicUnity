using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public float shotDelay;                 // ���� ������
    public float pursuitMoveDelay;          // ���� ������
    public float wanderMoveDelay;           // ��Ȳ ������

    public Vector2 directionOfGaze;         // ���� �ٶ󺸰� �ִ� ����
    public Vector2 myFront;                 // �� ������⿡ ���� �ִ��� �����ϴ� ����

    public int fieldOfView = 0;             // �þ߹���
    public int attackOfView = 0;            // ���ݹ���


    [Space]
    [Header("State")]

    public bool isFindPlayer = false;       // �÷��̾ ã�Ҵ���
    public bool isAbleToAttack = false;     // �÷��̾ ���� ������ ���Դ���
    public bool isTouchOutsideWall = false; // �ܰ����� �꿴����



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;

    }




    void Update()
    {
        otherTasks();

        move();

        manage();

        rayWork();

    }


    void otherTasks()
    {
        isAbleToAttack = Vector2.Distance(targetPos.position, transform.position) < attackOfView;
        isFindPlayer = Vector2.Distance(targetPos.position, transform.position) < fieldOfView;

        if (directionOfGaze.x > 0) spriteRenderer.flipX = true;
        else if (directionOfGaze.x < 0) spriteRenderer.flipX = false;
        else
        {
            if (transform.position.x - targetPos.position.x > 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;

        }
    }



    void move()
    {
        if (isTouchOutsideWall)
        {
            directionOfGaze *= (-1);
        }

        if (isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("pursuitPlayer");

        }
        else if (!isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("wander");

        }

        rigid.velocity = new Vector2(directionOfGaze.x * speed, directionOfGaze.y * speed);

    }

    IEnumerator wander()
    {
        while (true)
        {
            if(isTouchOutsideWall)
            {
                yield return null;
                continue;

            }

            directionOfGaze = new Vector2(Random.Range(-30, 31), Random.Range(-30, 31)).normalized;
            yield return new WaitForSeconds(wanderMoveDelay);

            if (isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }
        }
    }


    IEnumerator pursuitPlayer()
    {
        while (true)
        {
            if(isTouchOutsideWall)
            {
                yield return null;
                continue;

            }

            directionOfGaze = new Vector2(targetPos.position.x - transform.position.x
                , targetPos.position.y - transform.position.y).normalized;

            Vector2 tempVec = directionOfGaze;
            if(isAbleToAttack)
            {
                directionOfGaze = new Vector2(0, 0);
                yield return new WaitForSeconds(shotDelay);
                shot();

            }
            directionOfGaze = tempVec;
            yield return new WaitForSeconds(pursuitMoveDelay);

            if (!isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }
        }
    }

    void shot()
    {
        Vector2 tempVec = directionOfGaze;

        Vector2 bulletPosision = new Vector2(transform.position.x, transform.position.y);
        GameObject instantBullet = Instantiate(bullet, bulletPosision, new Quaternion());

        EnemyBullet bulletScript = instantBullet.GetComponent<EnemyBullet>();
        bulletScript.setType(EnemyBullet.Type.Fly);
        float bulletSpeed = bulletScript.bulletSpeed;

        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity =
            new Vector2(
                (targetPos.position - transform.position).normalized.x * bulletSpeed,
                (targetPos.position - transform.position).normalized.y * bulletSpeed);
    }



    // �ӽ÷� ��� �ٿ��� ����! �����ص� ��!!
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }

    }

    void rayWork()
    {
        Vector2 myPosition = transform.position;

        if(directionOfGaze.x < 0)
        {
            if (directionOfGaze.y < 0) myFront = new Vector2(-1, -1);
            else myFront = new Vector2(-1, 1);
        }
        else
        {
            if (directionOfGaze.y < 0) myFront = new Vector2(1, -1);
            else myFront = new Vector2(1, 1);
        }

        Collider2D myAround = Physics2D.OverlapBox(
            myPosition + myFront, new Vector2(1, 1), 0, groundLayer);


        if (myAround != null && myAround.tag == "OutsideWall")
            isTouchOutsideWall = true;
        else 
            isTouchOutsideWall = false;


    }



    void OnDrawGizmos()
    {
        if (rigid == null) return;
        Vector2 myPosition = transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(myPosition + myFront, new Vector2(1,1));

    }


    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }














































}
