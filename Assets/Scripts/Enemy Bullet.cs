using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int damage;

    void Awake()
    {
        StartCoroutine("goToDie");
        
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
