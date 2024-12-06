using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBossBullet : MonoBehaviour
{
    public enum BulletType { Repeat, Fast, Multi };
    public BulletType type;

    public float[] damages;
    public float myDamage;

    public float[] speeds;
    public float mySpeed;

    void Awake()
    {
        if (type == BulletType.Repeat)
        {
            myDamage = damages[0];
            mySpeed = speeds[0];

        }
        else if (type == BulletType.Fast)
        {
            myDamage = damages[1];
            mySpeed = speeds[1];

        }
        else
        {
            myDamage = damages[2];
            mySpeed = speeds[2];

        }

        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);

        }
        else if (collision.gameObject.tag == "OutsideWall")
        {
            Destroy(gameObject);

        }

    }

}
