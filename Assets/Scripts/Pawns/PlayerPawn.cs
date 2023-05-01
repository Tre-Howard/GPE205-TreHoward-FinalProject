using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : Pawn
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    //private float audioDelayTime = 2f;
    //private float startDelayTime = 3f;

    // Start is called before the first frame update
    public override void Start()
    {

    }

    // Update is called once per frame
    public override void Update()
    {
        base.Start(); // calls from parent
    }


    // Movement

    public override void MoveForward()
    {
        mover.Move(transform.forward, moveSpeed);
    }

    public override void MoveBackward()
    {
        mover.Move(transform.forward, -moveSpeed);
    }

    public override void RotateClockwise() // as discussed via teams with the teacher, this is not working with 2 confirmed working codes and still not working
    {
        mover.Rotate(turnSpeed);
    }

    public override void RotateCounterClockwise() // as discussed via teams with the teacher, this is not working with 2 confirmed working codes and still not working
    {
        mover.Rotate(-turnSpeed);
    }


    // Shooting/Guns/ETC

    public override void Shoot() // shoot function for tank, with cooldown that counts up or "are we there yet" countdown, firerate is based on RPS
    {
        if (shooter == null)
        {

        }
        else
        {
            shooter.Shoot(shellPrefab, fireForce, damageDone, shellLifespan);
        }
    }

    public override void Melee()
    {
        return;
    }


    private void OnDestroy()
    {
        Controller controller = this.GetComponentInChildren<Controller>();

        if (this == null)
        {
            DestroyImmediate(this); // this is just in case editor doesnt get rid of old instanatiations
        }
        else
        {
            Destroy(this);
        }
    }



    // AI Code
    public override void RotateTowards(Vector3 targetPosition)
    {
        Vector3 vectorToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

    }
}
