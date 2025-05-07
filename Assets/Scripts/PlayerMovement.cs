using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float runSpeed;
    public int walkCount;
    private Vector3 vector;
    private float applyRunSpeed;
    private int currentWalkCount;
    public static bool canMove = true;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!canMove)
            return;

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                applyRunSpeed = runSpeed;
            } else
                applyRunSpeed = 0;

            vector.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), transform.position.z);
            if(vector.x != 0)
            {
                transform.Translate(vector.x * (speed + applyRunSpeed), 0, 0);
            } else if(vector.y != 0)
            {
                transform.Translate(0, vector.y * (speed + applyRunSpeed), 0);
            }

            currentWalkCount++;
        }
        currentWalkCount = 0;
    }
}
