using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Some : MonoBehaviour
{
    public Transform player;   // �÷��̾��� Transform
    public Transform gun;      // ���� Transform
    public Transform unit;     // A ������ Transform

    public float radius = 1.5f;      // ���� ���� ������
    public float rotationSpeed = 1f; // ���� ���� �̵� �ӵ�

    private float angle = 0f; // ���� ����

    void Update()
    {
        Vector2 diff = unit.position - player.position;
        float angle = Mathf.Atan2(diff.x, diff.y);

        float x = unit.position.x + (-1) * (Mathf.Sin(angle) * radius);
        float y = unit.position.y + (-1) * (Mathf.Cos(angle) * radius);

        gun.position = new Vector2(x, y);

        // 2. �÷��̾ ���� �� ȸ��
        //Vector2 direction = (Vector2)(player.position - gun.position);
        //float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //gun.rotation = Quaternion.Euler(0, 0, targetAngle);
    }



    void OnDrawGizmos()
    {
        Gizmos.DrawCube(gun.position, new Vector3(0.25f, 0.25f));


    }


}


















