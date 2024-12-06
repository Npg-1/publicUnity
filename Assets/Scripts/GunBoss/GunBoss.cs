using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBoss : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Coroutine currentCoroutine;
    private Transform targetPos;
    private Transform inPlace;


    [Header("Manage")]
    public bool stopPosition;

    [Space]
    public GameObject target;
    public GameObject repeatBullet;
    public GameObject fastBullet;
    public GameObject multiBullet;

    public GameObject gaugesObject;
    public GameObject gaugeObject;

    public GameObject pointGun;
    public Transform[] pointGuns;
    public Transform[] bossPositions;


    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;

    public float repeatShotDelay;

    public float gaugeAmount;

    public float pointGunRadius;
    public float multiShotDelay;

    public float waitTime;


    [Space]
    [Header("State")]
    public bool isReturnInPlace = false;
    public bool isWaiting = false;
    public bool isRepeatShot = false;
    public bool isFastShot = false;
    public bool isMulitShot = false;


    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        target = GameObject.Find("Player");

        targetPos = target.transform;
        inPlace = bossPositions[1];

        StartCoroutine(floatfloat());
        StartCoroutine(tempCoroutine());

    }


    IEnumerator tempCoroutine()
    {
        int count = 0;
        while(true)
        {
            if (isReturnInPlace || isRepeatShot || isFastShot || isMulitShot || isWaiting)
            {
                yield return null;
                continue;

            }

            switch(count)
            {
                case 0:
                    repeatShot();
                    break;
                case 1:
                    fastShot();
                    break;
                case 2:
                    multiShot();
                    break;
                case 3:
                    hp = 150;
                    repeatShot();
                    break;
                case 4:
                    fastShot();
                    break;
                case 5:
                    multiShot();
                    break;
                case 6:
                    hp = 50;
                    repeatShot();
                    break;
                case 7:
                    fastShot();
                    break;
                case 8:
                    multiShot();
                    break;
            }
            count++;



        }
    }






    void Update()
    {
        //think();
        settingPointGunsPosNRotation();
        manage();

    }


    void think()
    {
        if (isReturnInPlace || isRepeatShot || isFastShot || isMulitShot || isWaiting)
            return;

        int ranNum = Random.Range(0, 3);
        switch (ranNum)
        {
            case 0:
                {
                    repeatShot();
                    break;

                }
            case 1:
                {
                    fastShot();
                    break;

                }
            case 2:
                {
                    multiShot();
                    break;
                }
        }

    }





    void repeatShot()
    {
        if (hp < 100) { StartCoroutine("doRepeatShot", 30); }
        else if (hp < 200) { StartCoroutine("doRepeatShot", 15); }
        else if (hp >= 200) { StartCoroutine("doRepeatShot", 10); }

    }

    IEnumerator doRepeatShot(int repeatShotAmount)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;


        isRepeatShot = true;
        while (true)
        {
            if (Vector2.Distance(bossPositions[0].position, transform.position) < 0.1f)
                break;

            Vector2 direction = new Vector2(bossPositions[0].position.x - transform.position.x
                , bossPositions[0].position.y - transform.position.y).normalized;

            rigid.velocity = direction * speed;
            yield return null;

        }

        rigid.velocity = new Vector2();
        yield return new WaitForSeconds(1f);

        float alreayFiredAmount = 0;
        while (alreayFiredAmount <= repeatShotAmount)
        {
            yield return new WaitForSeconds(repeatShotDelay);

            Vector3 diff = transform.position - targetPos.position;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

            GameObject instanceRepeatBullet = Instantiate(repeatBullet, transform.position,
                Quaternion.Euler(0, 0, angle));

            Rigidbody2D bulletRigid = instanceRepeatBullet.GetComponent<Rigidbody2D>();
            GunBossBullet bulletScript = instanceRepeatBullet.GetComponent<GunBossBullet>();

            Vector3 bulletDirection = new Vector3(targetPos.position.x - transform.position.x,
                targetPos.position.y - transform.position.y).normalized;

            bulletRigid.AddForce(bulletDirection * bulletScript.mySpeed, ForceMode2D.Impulse);

            alreayFiredAmount += 1;
        }

        yield return new WaitForSeconds(0.75f);
        isRepeatShot = false;
        StartCoroutine("returnInPlace");


    }














    void fastShot()
    {
        if (hp < 100) { StartCoroutine(doFastShot(5, 1.4f)); }
        else if (hp < 200) { StartCoroutine(doFastShot(4, 1.2f)); }
        else if (hp >= 200) { StartCoroutine(doFastShot(3, 1f)); }

    }

    IEnumerator doFastShot(int shotCount, float multiSpeed)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        isFastShot = true;
        yield return new WaitForSeconds(waitTime);

        while (true)
        {
            if (Vector2.Distance(inPlace.position, transform.position) < 0.1f)
                break;

            Vector2 direction = new Vector2(inPlace.position.x - transform.position.x
                , inPlace.position.y - transform.position.y).normalized;

            rigid.velocity = direction * speed;
            yield return null;

        }

        rigid.velocity = new Vector2();

        yield return new WaitForSeconds(0.75f);
        gaugesObject.SetActive(true);

        for (int i = 0; i < shotCount; i++)
        {
            float gaugeScaleX = 0;
            while (gaugeScaleX <= 1.35)
            {
                gaugeObject.transform.localScale = new Vector3(gaugeScaleX, 0.3f, 1);
                gaugeScaleX += 0.05f;
                yield return new WaitForSeconds(gaugeAmount);

            }

            Vector3 diff = transform.position - targetPos.position;
            float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 180;

            GameObject instanceFastBullet = Instantiate(fastBullet, transform.position,
                Quaternion.Euler(0, 0, angle));

            Rigidbody2D bulletRigid = instanceFastBullet.GetComponent<Rigidbody2D>();
            GunBossBullet bulletScript = instanceFastBullet.GetComponent<GunBossBullet>();

            Vector3 bulletDirection = new Vector3(targetPos.position.x - transform.position.x,
                targetPos.position.y - transform.position.y).normalized;


            float bulletSpeed = bulletScript.mySpeed * multiSpeed;
            bulletRigid.AddForce(bulletDirection * bulletSpeed, ForceMode2D.Impulse);

        }



        gaugesObject.SetActive(false);
        isFastShot = false;

    }
















    void multiShot()
    {
        if (hp < 100) { StartCoroutine(doMultiShot(3)); }
        else if (hp < 200) { StartCoroutine(doMultiShot(2)); }
        else if (hp >= 200) { StartCoroutine(doMultiShot(1)); }

    }

    IEnumerator doMultiShot(int step)
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;

        isMulitShot = true;
        yield return new WaitForSeconds(waitTime);

        float[] distances = new float[6];
        for (int i = 1; i < 6; i++)
            distances[i] = Vector2.Distance(bossPositions[i].position, targetPos.position);

        float minDistance = float.MaxValue;
        for (int i = 1; i < 6; i++)
            minDistance = (distances[i] < minDistance) ? distances[i] : minDistance;

        int idx = -1;
        for (int i = 1; i < 6; i++)
            if (distances[i] == minDistance) idx = i;



        while (true)
        {
            if (Vector2.Distance(bossPositions[idx].position, transform.position) < 0.1f)
                break;

            Vector2 direction = new Vector2(bossPositions[idx].position.x - transform.position.x
                , bossPositions[idx].position.y - transform.position.y).normalized;

            rigid.velocity = direction * speed;
            yield return null;

        }

        rigid.velocity = new Vector2();


        switch (step)
        {
            case 1:
                {
                    for(int i = 0; i < 3; i++)
                    {
                        yield return new WaitForSeconds(multiShotDelay);

                        GameObject instanceMulitBullet1 = Instantiate(multiBullet, pointGuns[4].position, new Quaternion());
                        GameObject instanceMulitBullet2 = Instantiate(multiBullet, pointGun.transform.position, new Quaternion());
                        GameObject instanceMulitBullet3 = Instantiate(multiBullet, pointGuns[5].position, new Quaternion());

                        Rigidbody2D bulletRigid1 = instanceMulitBullet1.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid2 = instanceMulitBullet2.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid3 = instanceMulitBullet3.GetComponent<Rigidbody2D>();

                        Vector3 bulletDirection1 = (pointGuns[4].position - transform.position).normalized;
                        Vector3 bulletDirection2 = (pointGun.transform.position - transform.position).normalized;
                        Vector3 bulletDirection3 = (pointGuns[5].position - transform.position).normalized;

                        GunBossBullet bulletScript = instanceMulitBullet1.GetComponent<GunBossBullet>();
                        float bulletSpeed = bulletScript.mySpeed;

                        bulletRigid1.AddForce(bulletDirection1 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid2.AddForce(bulletDirection2 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid3.AddForce(bulletDirection3 * bulletSpeed, ForceMode2D.Impulse);

                    }
                    break;
                }
            case 2:
                {
                    for(int i = 0; i < 3; i++)
                    {
                        yield return new WaitForSeconds(multiShotDelay);

                        GameObject instanceMulitBullet1 = Instantiate(multiBullet, pointGuns[0].position, new Quaternion());
                        GameObject instanceMulitBullet2 = Instantiate(multiBullet, pointGuns[1].position, new Quaternion());
                        GameObject instanceMulitBullet3 = Instantiate(multiBullet, pointGuns[2].position, new Quaternion());
                        GameObject instanceMulitBullet4 = Instantiate(multiBullet, pointGuns[3].position, new Quaternion());

                        Rigidbody2D bulletRigid1 = instanceMulitBullet1.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid2 = instanceMulitBullet2.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid3 = instanceMulitBullet3.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid4 = instanceMulitBullet4.GetComponent<Rigidbody2D>();

                        Vector3 bulletDirection1 = (pointGuns[0].position - transform.position).normalized;
                        Vector3 bulletDirection2 = (pointGuns[1].position - transform.position).normalized;
                        Vector3 bulletDirection3 = (pointGuns[2].position - transform.position).normalized;
                        Vector3 bulletDirection4 = (pointGuns[3].position - transform.position).normalized;

                        GunBossBullet bulletScript = instanceMulitBullet1.GetComponent<GunBossBullet>();
                        float bulletSpeed = bulletScript.mySpeed;

                        bulletRigid1.AddForce(bulletDirection1 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid2.AddForce(bulletDirection2 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid3.AddForce(bulletDirection3 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid4.AddForce(bulletDirection4 * bulletSpeed, ForceMode2D.Impulse);

                    }



                    break;
                }
            case 3:
                {
                    for (int i = 0; i < 3; i++)
                    {
                        yield return new WaitForSeconds(multiShotDelay);

                        GameObject instanceMulitBullet1 = Instantiate(multiBullet, pointGuns[0].position, new Quaternion());
                        GameObject instanceMulitBullet2 = Instantiate(multiBullet, pointGuns[1].position, new Quaternion());
                        GameObject instanceMulitBullet3 = Instantiate(multiBullet, pointGuns[2].position, new Quaternion());
                        GameObject instanceMulitBullet4 = Instantiate(multiBullet, pointGuns[3].position, new Quaternion());
                        GameObject instanceMulitBullet5 = Instantiate(multiBullet, pointGuns[4].position, new Quaternion());
                        GameObject instanceMulitBullet6 = Instantiate(multiBullet, pointGun.transform.position, new Quaternion());
                        GameObject instanceMulitBullet7 = Instantiate(multiBullet, pointGuns[5].position, new Quaternion());

                        Rigidbody2D bulletRigid1 = instanceMulitBullet1.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid2 = instanceMulitBullet2.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid3 = instanceMulitBullet3.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid4 = instanceMulitBullet4.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid5 = instanceMulitBullet5.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid6 = instanceMulitBullet6.GetComponent<Rigidbody2D>();
                        Rigidbody2D bulletRigid7 = instanceMulitBullet7.GetComponent<Rigidbody2D>();

                        Vector3 bulletDirection1 = (pointGuns[0].position - transform.position).normalized;
                        Vector3 bulletDirection2 = (pointGuns[1].position - transform.position).normalized;
                        Vector3 bulletDirection3 = (pointGuns[2].position - transform.position).normalized;
                        Vector3 bulletDirection4 = (pointGuns[3].position - transform.position).normalized;
                        Vector3 bulletDirection5 = (pointGuns[4].position - transform.position).normalized;
                        Vector3 bulletDirection6 = (pointGun.transform.position - transform.position).normalized;
                        Vector3 bulletDirection7 = (pointGuns[5].position - transform.position).normalized;


                        GunBossBullet bulletScript = instanceMulitBullet1.GetComponent<GunBossBullet>();
                        float bulletSpeed = bulletScript.mySpeed;

                        bulletRigid1.AddForce(bulletDirection1 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid2.AddForce(bulletDirection2 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid3.AddForce(bulletDirection3 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid4.AddForce(bulletDirection4 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid5.AddForce(bulletDirection5 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid6.AddForce(bulletDirection6 * bulletSpeed, ForceMode2D.Impulse);
                        bulletRigid7.AddForce(bulletDirection7 * bulletSpeed, ForceMode2D.Impulse);

                    }


                    break;
                }
        }

        yield return new WaitForSeconds(1f);

        isMulitShot = false;
        StartCoroutine(returnInPlace());

    }



    void settingPointGunsPosNRotation()
    {
        Vector2 diff = transform.position - targetPos.position;
        float angle = Mathf.Atan2(diff.x, diff.y);

        float x = transform.position.x + (-1) * (Mathf.Sin(angle) * pointGunRadius);
        float y = transform.position.y + (-1) * (Mathf.Cos(angle) * pointGunRadius);

        pointGun.transform.position = new Vector2(x, y);


        diff = transform.position - targetPos.position;
        angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90;

        pointGun.transform.rotation = Quaternion.Euler(0, 0, angle);



    }

































    IEnumerator returnInPlace()
    {
        isReturnInPlace = true;
        while (true)
        {
            if (Vector2.Distance(inPlace.position, transform.position) < 0.1f)
                break;

            Vector2 direction = new Vector2(inPlace.position.x - transform.position.x
                , inPlace.position.y - transform.position.y).normalized;

            rigid.velocity = direction * speed;
            yield return null;

        }

        isReturnInPlace = false;
        rigid.velocity = new Vector2(0, 0);

    }


    IEnumerator floatfloat()
    {
        while (true)
        {
            if (isRepeatShot || isFastShot || isMulitShot)
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













































