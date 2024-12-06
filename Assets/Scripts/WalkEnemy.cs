using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Coroutine currentCoroutine;
    private Transform targetPos;
    private Animator anim;


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
    public float shotAnimTime;

    public int directionOfGaze = 0;
    public int fieldOfView = 0;

    public bool isWalk = false;
    public bool isAttack = false;



    [Space]
    [Header("State")]

    public bool isFindPlayer = false;
    public bool isCliff;
    public bool isFalling;



    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        target = GameObject.Find("Player");
        targetPos = target.transform;

    }


    void Update()
    {
        findPlayer_flip();  
        move();             
        rayWork();          
        manage();          
        
        // 연속적으로 실해하는 부분이 Update()

    }





    void findPlayer_flip()
    {
        isFindPlayer = Vector2.Distance(targetPos.position, transform.position) < fieldOfView;

        if(!isFalling)
        {
            //spriteRenderer.flipX = (directionOfGaze < 0);
            if (directionOfGaze < 0) spriteRenderer.flipX = true;
            else if (directionOfGaze > 0) spriteRenderer.flipX = false;
            else
            {
                // 내가 플레이어 왼쪽
                if (transform.position.x - targetPos.position.x < 0) spriteRenderer.flipX = false;
                else spriteRenderer.flipX = true;

            }

        }
    }




    void move()
    {
        if(isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("pursuitPlayer");

        }
        else if(!isFindPlayer && currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine("wander");

        }

        if (!isFalling && isCliff) directionOfGaze *= -1;


        rigid.velocity = new Vector2(directionOfGaze * speed, rigid.velocity.y);

    }

    IEnumerator pursuitPlayer()
    {
        while(true)
        {
            if (isFalling)
            {
                yield return null;
                continue;
            }

            directionOfGaze = transform.position.x > targetPos.position.x ? -1 : 1;

            isWalk = true;
            anim.SetBool("isWalk", true);
            
            StartCoroutine("shot");
            yield return new WaitForSeconds(shotDelay);


            if(!isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }
        }
    }

    IEnumerator shot()
    {
        isWalk = false;
        isAttack = true;
        anim.SetBool("isWalk", false);
        anim.SetBool("isAttack", true);

        int tempDirection = directionOfGaze;
        directionOfGaze = 0;

        yield return new WaitForSeconds(shotAnimTime);

        Vector3 diff = transform.position - targetPos.position;
        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        Vector2 bulletPosision = new Vector2(transform.position.x + directionOfGaze / 1.5f, transform.position.y);
        GameObject instantBullet = Instantiate(bullet, bulletPosision, Quaternion.Euler(0, 0, angle + 180));


        Rigidbody2D bulletRigid = instantBullet.GetComponent<Rigidbody2D>();
        bulletRigid.velocity =
            new Vector2(
                (targetPos.position - transform.position).normalized.x * bulletSpeed,
                (targetPos.position - transform.position).normalized.y * bulletSpeed);

        isWalk = true;
        isAttack = false;
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack", false);

        directionOfGaze = tempDirection;

    }

    IEnumerator wander()
    {
        while(true)
        {
            if (isFalling)
            {
                yield return null;
                continue;
            }

            directionOfGaze = 0;
            isWalk = false;
            anim.SetBool("isWalk", false);
            yield return new WaitForSeconds(Random.Range(0, 5) / 5f);


            if (Random.Range(0, 2) == 0) directionOfGaze = -1;
            else directionOfGaze = 1;

            isWalk = true;
            anim.SetBool("isWalk", true);
            yield return new WaitForSeconds(3f);


            if(isFindPlayer)
            {
                currentCoroutine = null;
                yield break;

            }
        }
    }


    // 임시로 잠시 붙여둔 거임! 삭제해도 됨!!
    void OnCollisionEnter2D(Collision2D collision) {
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
        Gizmos.DrawWireCube(myPosition + new Vector2(directionOfGaze/2f, -0.5f),
            new Vector2(0, 1));

        Gizmos.DrawWireCube(myPosition + new Vector2(0, -1.0f),
            new Vector2(0, 1));

    }


    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }
}


















































