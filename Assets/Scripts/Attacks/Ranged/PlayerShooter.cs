using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooter : Shooter
{
    public Transform firepointTransform;

    public float timerDelay;
    private float timeUntilNextEvent;
    public Boolean canShoot;

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
            canShoot = true;
        }
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        // variable timeDelay = fireRate;
        ShootCooldown(); //timeDelay

        if (canShoot == true)
        {
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position, firepointTransform.rotation) as GameObject; // can shoot, move forward and create shell
            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();

            if (doh != null) //check if damage is needed, if so apply
            {
                doh.damageDone = damageDone;
                doh.owner = GetComponent<Pawn>();
            }

            Rigidbody rb = newShell.GetComponent<Rigidbody>(); // on spawn, move forward this variables #

            if (rb != null)
            {
                rb.AddForce(firepointTransform.forward * fireForce);
            }

            canShoot = false;
            Destroy(newShell, lifespan); // either hit or after lifespan, destroy tank shell


            // do this part when your ready, get everything else up first
            if (audioSource != null)
            {
                audioSource.PlayOneShot(playerPawn.audioClips[0]);
            }
            else
            {
                Debug.Log("audioSource in TankShooter failed");

            }

        }
        else
        {
            canShoot = false;
            return;
        }
    }

    public void ShootCooldown()
    {

        if (canShoot == true && timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = timerDelay; // = fireRate

        }
        else
        {
            canShoot = false;
            return;
        }
    }
}
