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
    public GameObject slashObject;
    public GameObject swordObject;

    [Space]
    public LayerMask layers;
    public Transform[] inPlaces;
    public Transform curInPlace;
    public Transform[] slashSummonPositions;
    public Transform[] swordSummonPositions;




    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;
    public int directionOfGaze = 0;

    public float slashSpeed;
    public float slashDelay;

    public float rushSpeed;
    public float rushDelay;

    public float summonSwordSpeed;
    public float summonSwordDelay;

    public float returnInPlaceSpeed;
    public float skillDelay;



    [Space]
    [Header("State")]
    public bool isReturnInPlace = false;
    public bool isInInPlace = false;
    public bool isRush = false;
    public bool isSlash = false;
    public bool isSummonSword = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameObject.Find("Player");
        targetPos = target.transform;

        StartCoroutine("floatfloat");
        //StartCoroutine("tempCouroutine");


    }

    IEnumerator tempCouroutine()
    {
        hp = 300;
        Invoke("rush", 1);
        Invoke("slash", 3);
        Invoke("summonSword", 5);
        yield return new WaitForSeconds(11.5f);

        hp = 150;
        Invoke("rush", 1);
        Invoke("slash", 6);
        Invoke("summonSword", 6);
        yield return new WaitForSeconds(12f);

        hp = 50;
        Invoke("rush", 1);
        Invoke("slash", 7);
        Invoke("summonSword", 10);

    }


    void Update()
    {
        rayWork();
        manage();
        think();

    }


    void think()
    {
        if(isReturnInPlace || isRush || isSlash || isSummonSword)
            return;

        int ranNum = Random.Range(0, 3);
        switch (ranNum)
        {
            case 0:
                {
                    rush();
                    break;

                }
            case 1:
                {
                    slash();
                    break;

                }
            case 2:
                {
                    summonSword();
                    break;
                }
        }

    }




    void summonSword()
    {
        if (hp < 100) { StartCoroutine("doSummonSword", 4); }
        else if (hp < 200) { StartCoroutine("doSummonSword", 2); }
        else if (hp >= 200) { StartCoroutine("doSummonSword", 1); }

    }

    IEnumerator doSummonSword(int count)
    {
        // 왼쪽: 0, 1, 2, 3, 4, 5
        // 오른쪽: 6, 7, 8, 9, 10, 11
        isSummonSword = true;
        yield return new WaitForSeconds(skillDelay);
        yield return new WaitForSeconds(summonSwordDelay);

        int[] ranArr;
        if(Vector3.Distance(targetPos.position, swordSummonPositions[1].position) < 
            Vector3.Distance(targetPos.position, swordSummonPositions[11].position))
        {
            // 플레이어가 왼쪽에 좀 더 가깝다면, 사용할 수 있는 위치는 0~5까지
            ranArr = calRanNum(count, true);
        }
        else
        {
            // 플레이어가 오른쪽에 좀 더 가깝다면, 사용할 수 있는 위치는 6~11까지
            ranArr = calRanNum(count, false);

        }

        for (int i = 0; i < count; i++)
        {
            float ranSeconds = Random.Range(0.25f, 1f);
            yield return new WaitForSeconds(ranSeconds);

            Vector3 tempVec = swordSummonPositions[ranArr[i]].position;
            Vector3 tempGakDo = new Vector3(tempVec.x - targetPos.position.x, 
                tempVec.y - targetPos.position.y).normalized;

            Vector3 diff = tempVec - targetPos.position;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            GameObject instantSword = Instantiate(swordObject, 
                swordSummonPositions[ranArr[i]].position, Quaternion.Euler(0, 0, angle + 90));

            Rigidbody2D swordRigid = instantSword.GetComponent<Rigidbody2D>();
            swordRigid.AddForce(tempGakDo * 2 * (-1), ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.5f);
            swordRigid.AddForce(tempGakDo * summonSwordSpeed * (-1), ForceMode2D.Impulse);
        }

        isSummonSword = false;
    }

    int[] calRanNum(int count, bool isLeft)
    {
        int[] arr = new int[count];
        int idx = 0;

        if(isLeft)
        {
            // 0~5
            Queue<int> queue = new Queue<int>();
            for (int i = 0; i < 6; i++) queue.Enqueue(i);

            for (int a1 = 0; a1 < count; a1++)
            { 
                int ranNum = Random.Range(0, 6);
                for (int i = 0; i < ranNum; i++) queue.Enqueue(queue.Dequeue());
                arr[idx] = queue.Dequeue();
                idx++;

            }
        }
        else
        {
            // 6~11
            Queue<int> queue = new Queue<int>();
            for (int i = 6; i < 12; i++) queue.Enqueue(i);

            for (int a1 = 0; a1 < count; a1++)
            {
                int ranNum = Random.Range(0, 6);
                for (int i = 0; i < ranNum; i++) queue.Enqueue(queue.Dequeue());
                arr[idx] = queue.Dequeue();
                idx++;

            }

        }

        return arr;

    }















    void slash()
    {
        if (hp < 100) { StartCoroutine("doSlash", 3); }
        else if (hp < 200) { StartCoroutine("doSlash", 2); }
        else if (hp >= 200) { StartCoroutine("doSlash", 1); }
    }


    IEnumerator doSlash(int count)
    {
        isSlash = true;
        yield return new WaitForSeconds(skillDelay);

        if(curInPlace.position.x == inPlaces[0].position.x)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(slashDelay);

                float s0 = Mathf.Abs(slashSummonPositions[0].position.y - targetPos.position.y);
                float s1 = Mathf.Abs(slashSummonPositions[1].position.y - targetPos.position.y);
                float s2 = Mathf.Abs(slashSummonPositions[2].position.y - targetPos.position.y);

                int idx = -1;
                if (s0 < s1 && s0 < s2) idx = 0;
                else if (s1 < s0 && s1 < s2) idx = 1;
                else if (s2 < s0 && s2 < s1) idx = 2;

                Vector3 tempVector = new Vector3(
                    slashSummonPositions[idx].position.x * (-1), slashSummonPositions[idx].position.y);

                while (true)
                {
                    if (Vector3.Distance(tempVector, transform.position) < 0.25f)
                        break;

                    Vector2 direction = new Vector2(tempVector.x - transform.position.x
                        , tempVector.y - transform.position.y).normalized;

                    rigid.velocity = direction * speed;
                    yield return null;

                }
                rigid.velocity = new Vector2(0, 0);

                GameObject instantSlash = Instantiate(slashObject, tempVector, new Quaternion());
                Rigidbody2D slashRigid = instantSlash.GetComponent<Rigidbody2D>();
                slashRigid.AddForce(new Vector2(slashSpeed, 0), ForceMode2D.Impulse);
            }
        }
        else if(curInPlace.position.x == inPlaces[1].position.x)
        {
            for (int i = 0; i < count; i++)
            {
                yield return new WaitForSeconds(slashDelay);

                float s0 = Mathf.Abs(slashSummonPositions[0].position.y - targetPos.position.y);
                float s1 = Mathf.Abs(slashSummonPositions[1].position.y - targetPos.position.y);
                float s2 = Mathf.Abs(slashSummonPositions[2].position.y - targetPos.position.y);

                int idx = -1;
                if (s0 < s1 && s0 < s2) idx = 0;
                else if (s1 < s0 && s1 < s2) idx = 1;
                else if (s2 < s0 && s2 < s1) idx = 2;


                Vector3 tempVector = new Vector3(
                    slashSummonPositions[idx].position.x, slashSummonPositions[idx].position.y);

                while (true)
                {
                    if (Vector3.Distance(tempVector, transform.position) < 0.25f)
                        break;

                    Vector2 direction = new Vector2(tempVector.x - transform.position.x
                        , tempVector.y - transform.position.y).normalized;

                    rigid.velocity = direction * speed;
                    yield return null;

                }
                rigid.velocity = new Vector2(0, 0);

                GameObject instantSlash = Instantiate(slashObject, tempVector, new Quaternion());
                Rigidbody2D slashRigid = instantSlash.GetComponent<Rigidbody2D>();
                slashRigid.AddForce(new Vector2((-1) * slashSpeed, 0), ForceMode2D.Impulse);
            }
        }
        isSlash = false;

        yield return new WaitForSeconds(0.5f);
        StartCoroutine("returnInPlace");

    }





















    void rush()
    {
        if (hp < 100) { StartCoroutine("doRush", 3); }
        else if (hp < 200) { StartCoroutine("doRush", 2); }
        else if (hp >= 200) { StartCoroutine("doRush", 1); }
    }


    IEnumerator doRush(int count)
    {
        isRush = true;
        yield return new WaitForSeconds(skillDelay);
        yield return new WaitForSeconds(rushDelay);

        for (int i = 0; i < count; i++)
        {
            Vector3 targetPosition = targetPos.position;
            Vector2 direction = new Vector2(targetPosition.x - transform.position.x,
                targetPosition.y - transform.position.y).normalized;
            rigid.AddForce(direction * rushSpeed, ForceMode2D.Impulse);

            while (true)
            {
                if (Vector3.Distance(targetPosition, transform.position) < 0.75f) break;
                yield return null;

            }

            rigid.velocity = new Vector2();

            if (i < count - 1) yield return new WaitForSeconds(rushDelay);
            else yield return new WaitForSeconds(0.25f);
        }
        isRush = false;
        StartCoroutine("returnInPlace");

    }

























    IEnumerator returnInPlace()
    {
        isReturnInPlace = true;
        if (Vector3.Distance(transform.position, inPlaces[0].position) 
            < Vector3.Distance(transform.position, inPlaces[1].position)) 
            curInPlace = inPlaces[0];
        else curInPlace = inPlaces[1];

        while (!isInInPlace)
        {
            Vector2 direction = new Vector2(curInPlace.position.x - transform.position.x
                , curInPlace.position.y - transform.position.y).normalized;

            rigid.velocity = direction * returnInPlaceSpeed;
            yield return null;

        }

        isReturnInPlace = false;
        rigid.velocity = new Vector2(0, 0);

    }


    void rayWork()
    {
        Vector2 myPosition = transform.position;
        Collider2D center = Physics2D.OverlapBox(
            myPosition, new Vector2(0.5f, 0.5f) , 0, layers);

        if (center != null)
        {
            if (center.tag == "BossInPlace")
            {
                isInInPlace = true;

            }
        }
        else
        {
            isInInPlace = false;

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
            if (isRush || isSlash || isReturnInPlace)
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


















































