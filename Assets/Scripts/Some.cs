using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Some : MonoBehaviour
{
    public Transform player;   // 플레이어의 Transform
    public Transform gun;      // 총의 Transform
    public Transform unit;     // A 유닛의 Transform

    public float radius = 1.5f;      // 원형 궤적 반지름
    public float rotationSpeed = 1f; // 총의 원형 이동 속도

    private float angle = 0f; // 현재 각도

    void Update()
    {
        Vector2 diff = unit.position - player.position;
        float angle = Mathf.Atan2(diff.x, diff.y);

        float x = unit.position.x + (-1) * (Mathf.Sin(angle) * radius);
        float y = unit.position.y + (-1) * (Mathf.Cos(angle) * radius);

        gun.position = new Vector2(x, y);

        // 2. 플레이어를 향해 총 회전
        //Vector2 direction = (Vector2)(player.position - gun.position);
        //float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //gun.rotation = Quaternion.Euler(0, 0, targetAngle);
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawCube(gun.position, new Vector3(0.25f, 0.25f));


    }


}


















