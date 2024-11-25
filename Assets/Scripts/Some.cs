using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Some : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D() »£√‚µ ");
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("OnCollisionStay2D() »£√‚µ ");
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D() »£√‚µ ");
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D() »£√‚µ ");
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("OnTriggerStay2D() »£√‚µ ");
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D() »£√‚µ ");
        
    }




}
