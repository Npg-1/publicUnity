using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Some : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D() ȣ���");
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("OnCollisionStay2D() ȣ���");
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("OnCollisionExit2D() ȣ���");
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D() ȣ���");
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("OnTriggerStay2D() ȣ���");
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("OnTriggerExit2D() ȣ���");
        
    }




}
