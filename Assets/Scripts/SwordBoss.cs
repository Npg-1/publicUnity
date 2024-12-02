using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBoss : MonoBehaviour
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
    public LayerMask layers;
    public Transform inplace;




    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;
    public float bulletSpeed;
    public float shotDelay;
    public int directionOfGaze = 0;

    public float rushSpeed;
    public float rushDelay;

    public float returnInPlaceSpeed;



    [Space]
    [Header("State")]
    public bool isRush = false;
    public bool isReturnInPlace = false;
    public bool isInInPlace = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;


        Invoke("think", 3);
        

    }


    void think()
    {
        StartCoroutine("rush");

    }





    IEnumerator returnInPlace()
    {
        isReturnInPlace = true;
        while (!isInInPlace)
        {
            Vector2 direction = new Vector2(inplace.position.x - transform.position.x
                , inplace.position.y - transform.position.y).normalized;

            Debug.Log("direction: " + direction);
            rigid.velocity = direction * returnInPlaceSpeed;
            yield return null;

        }
        isReturnInPlace = false;

    }




    IEnumerator rush()
    {
        isRush = true;
        if(hp < 100)
        {
            

        }
        else if(hp < 200)
        {

        }
        else if(hp >= 200)
        {
            doRush();
            yield return new WaitForSeconds(rushDelay);
            StartCoroutine("returnInPlace");

        }
        else { }
        isRush = false;

    }



    void doRush()
    {
        Vector2 direction = new Vector2(targetPos.position.x - transform.position.x
            , targetPos.position.y - transform.position.y).normalized;
        rigid.AddForce(direction * rushSpeed, ForceMode2D.Impulse);

    }





    void slash() { }

    void summonSword() { }



    void Update()
    {
        rayWork();

        manage();

    }





    void rayWork()
    {
        Vector2 myPosition = transform.position;
        Collider2D center = Physics2D.OverlapBox(myPosition, new Vector2(0.5f, 0.5f)
            , 0, layers);

        if (center != null)
        {
            if (center.tag == "BossInPlace")
            {
                isInInPlace = true;

            }
            else
            {
                isInInPlace = false;

            }
        }



    }



    void OnDrawGizmos()
    {
        if (rigid == null) return;

        Vector2 myPosition = transform.position;
        Gizmos.color = Color.white;

        Gizmos.DrawWireCube(myPosition, new Vector2(0.5f, 0.5f));

    }   




    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }
}


















































