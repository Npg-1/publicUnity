using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum Type { Walk, Idle, Fly, Bomb };
    private Type type;

    public float[] bulletSpeeds;
    public int damage;
    public float bulletSpeed;

    void Awake()
    {
        StartCoroutine("goToDie");
        
    }

    public void setType(Type type)
    {
        switch (type)
        {
            case Type.Walk:
                this.type = Type.Walk;
                bulletSpeed = bulletSpeeds[0];

                break;
            case Type.Idle:
                this.type = Type.Idle;
                bulletSpeed = bulletSpeeds[1];

                break;
            case Type.Fly:
                this.type = Type.Fly;
                bulletSpeed = bulletSpeeds[2];

                break;
            case Type.Bomb:
                this.type = Type.Bomb;
                bulletSpeed = bulletSpeeds[3];

                break;
        }
    }

    IEnumerator goToDie()
    {
        yield return new WaitForSeconds(6f);

        if (type == Type.Bomb) StartCoroutine(boom());
        else Destroy(gameObject);

    }

    IEnumerator boom()
    {
        yield return null;

        //Destroy(gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(type == Type.Bomb)
            StartCoroutine(boom());
        else
        {
            if(collision.gameObject.tag == "Player")
            {
                Destroy(gameObject);

            }
            else if(collision.gameObject.tag == "OutsideWall")
            {
                Destroy(gameObject);

            }
        }
    }

}

















































