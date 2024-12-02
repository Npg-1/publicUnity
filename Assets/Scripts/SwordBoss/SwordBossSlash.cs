using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordBossSlash : MonoBehaviour
{
    public int damage;

    void Awake()
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
