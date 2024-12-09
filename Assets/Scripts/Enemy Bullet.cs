using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public enum Type { Walk, Idle, Fly };
    private Type type;
    public float[] bulletSpeeds;


    public int damage;
    public float bulletSpeed;

    void Awake()
    {
        StartCoroutine("goToDie");
        
    }

    public void setType(int type)
    {
        switch (type)
        {
            case 0:
                this.type = Type.Walk;
                bulletSpeed = bulletSpeeds[0];

                break;
            case 1:
                this.type = Type.Idle;
                bulletSpeed = bulletSpeeds[1];

                break;
            case 2:
                this.type = Type.Fly;
                bulletSpeed = bulletSpeeds[2];

                break;

        }
    }

    IEnumerator goToDie()
    {
        yield return new WaitForSeconds(6f);
        Destroy(gameObject);

    }


    private void OnTriggerEnter2D(Collider2D collision)
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
