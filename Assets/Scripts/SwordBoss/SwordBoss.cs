using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject slashObject;
    public LayerMask layers;
    public Transform inplace;
    public Transform[] slashSummonPositions;




    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;
    public float slashSpeed;
    public float slashDelay;
    public int directionOfGaze = 0;

    public float rushSpeed;
    public float rushDelay;

    public float returnInPlaceSpeed;



    [Space]
    [Header("State")]
    public bool isRush = false;
    public bool isReturnInPlace = false;
    public bool isInInPlace = false;
    public bool isSlash = false;
    public bool isMoveSlashSummonPoint = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;


        StartCoroutine("floatfloat");
        Invoke("think", 1.5f);
        //Invoke("slash", 4.5f);
        //Invoke("slash", 7.5f);
        //Invoke("slash", 10.5f);
        //Invoke("slash", 13.5f);
        //Invoke("slash", 16.5f);
        //Invoke("slash", 19.5f);
        //Invoke("slash", 22.5f);
        //Invoke("slash", 25.5f);


    }


    IEnumerator think()
    {
        while (true)
        {
            Random random = new Random();
            int idx = random.Next(0, 2);

            switch()


        }
        

    }





    IEnumerator returnInPlace()
    {
        while (true)
        {
            if (isRush)
            {
                yield return null;
                continue;

            }

            isReturnInPlace = true;
            while (!isInInPlace)
            {
                Vector2 direction = new Vector2(inplace.position.x - transform.position.x
                    , inplace.position.y - transform.position.y).normalized;

                rigid.velocity = direction * returnInPlaceSpeed;
                yield return null;

            }

            isReturnInPlace = false;
            rigid.velocity = new Vector2(0, 0);
            break;

        }
    }


    void rush()
    {
        if (hp < 100)
        {
            StartCoroutine("doRush", 3);
            StartCoroutine("returnInPlace");


        }
        else if (hp < 200)
        {
            StartCoroutine("doRush", 2);
            StartCoroutine("returnInPlace");

        }
        else if (hp >= 200)
        {
            StartCoroutine("doRush", 1);
            StartCoroutine("returnInPlace");

        }
        else { }

    }


    IEnumerator doRush(int count)
    {
        isRush = true;

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPosition = targetPos.position;

            while (true)
            {
                if (Mathf.Abs(targetPosition.x - transform.position.x) < 0.25f
                    && Mathf.Abs(targetPosition.y - transform.position.y) < 0.25f)
                {
                    break;
                }

                Vector2 direction = new Vector2(targetPosition.x - transform.position.x,
                    targetPosition.y - transform.position.y).normalized;
                rigid.velocity = direction * rushSpeed;
                yield return null;

            }

            rigid.velocity = new Vector2();

            if(i < count-1)
                yield return new WaitForSeconds(rushDelay);
            else
                yield return new WaitForSeconds(0.5f);
        }
        isRush = false;


    }





    IEnumerator slash()
    {
        if (hp < 200)
        {
            for (int i = 0; i < 8; i++)
            {
                float s0 = Mathf.Abs(slashSummonPositions[0].position.y - targetPos.position.y);
                float s1 = Mathf.Abs(slashSummonPositions[1].position.y - targetPos.position.y);
                float s2 = Mathf.Abs(slashSummonPositions[2].position.y - targetPos.position.y);
                
                if(s0 < s1 && s0 < s2)
                {
                    StartCoroutine("moveSlashSummonPoint", 0);
                    StartCoroutine("doSlash", 0);
                }
                else if(s1 < s0 && s1 < s2)
                {
                    StartCoroutine("moveSlashSummonPoint", 1);
                    StartCoroutine("doSlash", 1);
                }
                else if(s2 < s0 && s2 < s1)
                {
                    StartCoroutine("moveSlashSummonPoint", 2);
                    StartCoroutine("doSlash", 2);
                }
                yield return new WaitForSeconds(slashDelay);
            }
        }
        else if (hp >= 200)
        {
            float s0 = Mathf.Abs(slashSummonPositions[0].position.y - targetPos.position.y);
            float s1 = Mathf.Abs(slashSummonPositions[1].position.y - targetPos.position.y);
            float s2 = Mathf.Abs(slashSummonPositions[2].position.y - targetPos.position.y);

            if(s0 < s1 && s0 < s2)
            {
                StartCoroutine("moveSlashSummonPoint", 0);
                StartCoroutine("doSlash", 0);
            }
            else if(s1 < s0 && s1 < s2)
            {
                StartCoroutine("moveSlashSummonPoint", 1);
                StartCoroutine("doSlash", 1);
            }
            else if(s2 < s0 && s2 < s1)
            {
                StartCoroutine("moveSlashSummonPoint", 2);
                StartCoroutine("doSlash", 2);
            }
        }
        else { }

        StartCoroutine("returnInPlace");
    }


    IEnumerator moveSlashSummonPoint(int idx)
    {
        isMoveSlashSummonPoint = true;
        while (true)
        {
            if (Mathf.Abs(slashSummonPositions[idx].position.x - transform.position.x) < 0.25f
                && Mathf.Abs(slashSummonPositions[idx].position.y - transform.position.y) < 0.25f)
            {
                break;
            }

            Vector2 direction = new Vector2(slashSummonPositions[idx].position.x - transform.position.x
                , slashSummonPositions[idx].position.y - transform.position.y).normalized;

            rigid.velocity = direction * speed;
            yield return null;

        }

        isMoveSlashSummonPoint = false;
        rigid.velocity = new Vector2(0, 0);

    }


    IEnumerator doSlash(int idx)
    {
        while (true)
        {
            if (isMoveSlashSummonPoint)
            {
                yield return null;
                continue;
            }

            isSlash = true;
            GameObject instantSlash = Instantiate(slashObject, slashSummonPositions[idx].position, new Quaternion());
            Rigidbody2D slashRigid = instantSlash.GetComponent<Rigidbody2D>();
            slashRigid.AddForce(new Vector2((-1) * slashSpeed, 0), ForceMode2D.Impulse);
            isSlash = false;
            break;

        }
    }






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


    IEnumerator floatfloat()
    {
        while (true)
        {
            if (isRush || isReturnInPlace)
            {
                yield return null;
                continue;
            }

            rigid.velocity = new Vector2(rigid.velocity.x, -0.5f);
            yield return new WaitForSeconds(0.5f);

            rigid.velocity = new Vector2(rigid.velocity.x, 0.5f);
            yield return new WaitForSeconds(0.5f);

        }
    }




    void manage()
    {
        if (stopPosition) rigid.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        else rigid.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;

    }
}


















































