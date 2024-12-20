using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEnemy : MonoBehaviour
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
    public int directionOfGaze = 0;
    public int fieldOfView = 0;
    public float jumpRange1 = 0;
    public float jumpRange2 = 0;


    [Space]
    [Header("State")]

    public bool isFindPlayer = false;
    public bool isCliff;
    public bool isFalling;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;

        StartCoroutine("floatPlatform");

    }

    void Update()
    {
        findPlayer_flip();

        move();

        rayWork();

        manage();


        //Debug.Log($"Current Coroutine: {currentCoroutine}");


    }


    void findPlayer_flip()
    {
        isFindPlayer = Vector2.Distance(targetPos.position, transform.position) < fieldOfView;

        if(!isFalling)
        {
            spriteRenderer.flipX = (directionOfGaze > 0);

        }

    }



    void move()
    {
        if (isFindPlayer && currentCoroutine == null)
        {
            //currentCoroutine = StartCoroutine("pursuitPlayer");

        }
        else if (!isFindPlayer && currentCoroutine == null)
        {
            //currentCoroutine = StartCoroutine("wander");

        }

        if (!isFalling && isCliff) directionOfGaze *= -1;
        rigid.velocity = new Vector2(directionOfGaze * speed, rigid.velocity.y);

    }




    IEnumerator floatPlatform()
    {
        int count = 0;
        //while (true)
        //{
        //    Debug.Log("count: ");
        //    yield return new WaitForSeconds(jumpRange1);
        //    rigid.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);

        //    yield return new WaitForSeconds(jumpRange2);
        //    rigid.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);

        //    count++;

        //}

        Debug.Log("sex");


        yield return null;


    }






    IEnumerator pursuitPlayer()
    {
        while(true)
        {
            if (isFalling)
            {
                Debug.Log("pursuitPlayer에서 지금 떨어지는중 반복 나감");
                continue;
            }
            directionOfGaze = transform.position.x > targetPos.position.x ? -1 : 1;
            

            StartCoroutine("shot");
            yield return new WaitForSeconds(shotDelay);


            if (!isFindPlayer)
            {
                Debug.Log("pursuitPlayer ending");
                currentCoroutine = null;
                break;

            }
        }
    }

    IEnumerator shot()
    {
        Vector2 bulletPosision = new Vector2(transform.position.x + directionOfGaze / 1.5f, transform.position.y);
        GameObject instantBullet = Instantiate(bullet, bulletPosision, new Quaternion());
        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity =
            new Vector2(
                (targetPos.position - transform.position).normalized.x * bulletSpeed,
                (targetPos.position - transform.position).normalized.y * bulletSpeed);

        yield return null;

    }

    IEnumerator wander()
    {
        while(true)
        {
            if (isFalling)
            {
                Debug.Log("wander에서 지금 떨어지는중 반복 나감");
                continue;
            }

            directionOfGaze = 0;
            yield return new WaitForSeconds(Random.Range(0, 5) / 5f);


            if (Random.Range(0, 2) == 0) directionOfGaze = -1;
            else directionOfGaze = 1;


            yield return new WaitForSeconds(3f);


            if(isFindPlayer)
            {
                Debug.Log("wander ending");
                currentCoroutine = null;
                break;

            }
        }
    }


    // 임시로 잠시 붙여둔 거임! 삭제해도 됨!!
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }
        
    }



    void rayWork()
    {
        Vector2 myPosition = transform.position;

        Collider2D frontBottom = Physics2D.OverlapBox(
            myPosition + new Vector2(directionOfGaze/2f, -0.5f),
            new Vector2(0, 1), 0, groundLayer);

        Collider2D myBottom = Physics2D.OverlapBox(
            myPosition + new Vector2(0, -1.0f),
            new Vector2(0, 1), 0, groundLayer);

        if (frontBottom == null) isCliff = true;
        else isCliff = false;

        if (myBottom == null) isFalling = true;
        else isFalling = false;

    }



    void OnDrawGizmos()
    {
        if (rigid == null) return;

        Vector2 myPosition = transform.position;

        Gizmos.color = Color.red;

        // Gizmos.DrawWireCube(Vector3 center, Vector3 size); 함수는 center인 중심으로 위치해서 size 크기만큼 그림
        Gizmos.DrawWireCube(myPosition + new Vector2(directionOfGaze/2f, -0.125f),
            new Vector2(0, 1.5f));

        Gizmos.DrawWireCube(myPosition + new Vector2(0, -1.25f),
            new Vector2(0, 1.5f));


    }


    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }
}
