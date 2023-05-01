using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed; // move speed variable
    protected float moveSpeedCache; // cache for movespeed to change gears in TankPawn
    public float turnSpeed; // turn speed variable
    public Mover mover; // variable to connect to a Mover component so my tank can move

    public Shooter shooter; // variable to connect to a TankShooter component so my tank can shoot
    public MeleeAttack melee;
    public GameObject shellPrefab;
    public GameObject meleePrefab;
    public float fireForce;
    public float damageDone;
    public float shellLifespan;

    public Controller controller;


    // Start is called before the first frame update
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
        shooter = GetComponent<Shooter>();
        moveSpeedCache = moveSpeed;
        controller = GetComponent<Controller>();
        melee = GetComponent<MeleeAttack>();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }


    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
    public abstract void Shoot();
    public abstract void Melee();

    // public abstract void ChangeGears(); - THIS IS EXTRA, finish project first


    // AI
    public abstract void RotateTowards(Vector3 targetPosition);
}
