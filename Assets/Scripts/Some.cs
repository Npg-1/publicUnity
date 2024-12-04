using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Some : MonoBehaviour
{
    public GameObject target;
    public Transform targetPos;

    void Awake()
    {

        Invoke("shoot", 1);

    }

    void shoot()
    {
        Vector3 u = targetPos.position;
        Vector3 me = transform.position;

        //Quaternion quaternion = Quaternion.Euler(u - me);   // X
        Vector3 A = me, B = u;

        //Vector3 diff = me - u;
        //Quaternion quaternion = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);   // X

        //Quaternion quaternion = Quaternion.Euler(0, 0, Vector3.Angle(me, u) * Mathf.Rad2Deg);   // X

        //GameObject bullet = Instantiate(target, transform.position, quaternion);  // X




        Vector3 direction = B - A;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Instantiate(target, A, Quaternion.Euler(0, 0, angle - 90));

    }


    void Update()
    {
        
    }


}
