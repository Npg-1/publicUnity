using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatEnemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Coroutine currentCoroutine;
    private Transform targetPos;


    [Header("Manage")]
    public bool stopPosition;

    [Space]
    public GameObject target;
    public GameObject bullet;
    public LayerMask groundLayer;

    [Space]
    [Header("Stats")]
    public float hp;
    public float speed;
    public float bulletSpeed;
    public float shotDelay;
    public int directionOfGaze = 0;
    public int fieldOfView = 0;


    [Space]
    [Header("State")]

    public bool isFindPlayer = false;
    public bool isCliff;
    public bool isFalling;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
