using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Some : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Awake()");
        StartCoroutine("WhileTrue");

        Invoke("SomeWhile", 5);

    }

    void SomeWhile()
    {
        while (true)
        {
            Debug.Log("SomeWhile()");
        }
    }

    IEnumerator WhileTrue()
    {
        Debug.Log("Five");
        yield return new WaitForSeconds(1f);

        Debug.Log("Four");
        yield return new WaitForSeconds(1f);

        Debug.Log("Three");
        yield return new WaitForSeconds(1f);

        Debug.Log("Two");
        yield return new WaitForSeconds(1f);

        Debug.Log("One");
        yield return new WaitForSeconds(1f);

        while (true)
        {
            Debug.Log("WhileTrue()");
            yield return null;

        }
    }

    private void Update()
    {
        Debug.Log("Update()");
        
    }


}
