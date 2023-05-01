using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeAttack : MeleeAttack
{
    public Transform firepointTransform;

    public float timerDelay;
    private float timeUntilNextEvent;
    public float attackRadius = 3f;
    public Boolean canAttack;

    private AudioSource audioSource;
    private PlayerPawn playerPawn;


    public override void Start() // on start, set up the next time you can shoot
    {

        timeUntilNextEvent = timerDelay;
        audioSource = GetComponent<AudioSource>();
        playerPawn = GetComponent<PlayerPawn>();

    }

    public override void Update() // every second check to see if i can shoot or not, if i can make my boolean true
    {
        timeUntilNextEvent -= Time.deltaTime;

        if (timeUntilNextEvent <= 0)
        {
            canAttack = true;
        }
    }

    public override void Melee(GameObject meleePrefab, float fireForce, float damageDone, float lifespan)
    {
        MeleeCooldown();

        if (canAttack)
        {
            GameObject newAttack = Instantiate(meleePrefab, firepointTransform.position, firepointTransform.rotation) as GameObject; // can shoot, move forward and create shell
            EnemyDamageOnHit doh = newAttack.GetComponent<EnemyDamageOnHit>();

            if (doh != null) //check if damage is needed, if so apply
            {
                doh.damageDone = damageDone;
                doh.owner = GetComponent<Pawn>();
                doh.targetTag = "Player"; // Set the target tag to "Player"
            }

            Rigidbody rb = newAttack.GetComponent<Rigidbody>(); // when spawning, u can add force to this attack if u want melee attacks
                                                                // to go forward, if not keep it at 0
            if (rb != null)
            {
                rb.AddForce(firepointTransform.forward * fireForce);
            }

            canAttack = false;
            Destroy(newAttack, lifespan);
            // audio stuff
            //noisemaker and sight stuff
        }
        else
        {
            canAttack = false;
            return;
        }
    }


    public void MeleeCooldown()
    {

        if (canAttack == true && timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = timerDelay; // = fireRate

        }
        else
        {
            canAttack = false;
            return;
        }
    }
}

