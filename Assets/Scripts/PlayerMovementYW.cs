using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementYW : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    private Vector3 vector;
    private float applyRunSpeed;
    private bool applyRunFlag = false;
    public int walkCount;
    private int currentWalkCount;
    public static bool canMove = true;
    //private Animator animator;

    void Start()
    {
        //animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
            }
            else
            {
                applyRunSpeed = 0;
            }

            vector.Set(moveX, moveY, 0);
            vector.Normalize(); // Normalize to avoid faster diagonal movement

            transform.Translate(vector * (speed + applyRunSpeed) * Time.deltaTime);
        }
    }
}
