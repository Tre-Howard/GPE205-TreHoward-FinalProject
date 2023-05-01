using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : MonoBehaviour // parent mover class
{
    public abstract void Start();
    public abstract void Move(Vector3 direction, float speed); // function to move forward/backward
    public abstract void Rotate(float turnSpeed); // function to rotate - NOT WORKING, CONFIRMED WITH TEACHER WITH TWO DIFFERENT ANSWERS THAT SHOULD WORK
}
