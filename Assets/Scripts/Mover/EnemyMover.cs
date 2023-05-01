using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMover : Mover // mover child class for tanks
{
    private Rigidbody rb; // get a connection to the rigidbody component

    // Start is called before the first frame update
    public override void Start()
    {
        rb = GetComponent<Rigidbody>(); // get the actual component and making this component reference the rb variable
    }

    public override void Move(Vector3 direction, float speed) // takes direction and speed, normalizes direction and multiplies by time.deltaTime to be
                                                              // PER SECOND instead of PER FRAME, times it by speed and thats its movement
                                                              // applies moveVector, or the new distance to the rigidbody and moves it forward (or backwards)
    {
        Vector3 moveVector = direction.normalized * speed * Time.deltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    public override void Rotate(float turnSpeed) //rotates tank
    {
        transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
    }

}
