using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // short version, when i use the HealthPickup code, it references a HealthPowerup variable
// this allows HealthPickup to see the public variables (healthToAdd) when looking at HealthPickup itself
public class PowerupHealth : Powerup
{

    public float healthToAdd;
    public float scoreToAdd; // new variable to store the score to add

    public override void Apply(PowerupManager target)
    {
        // apply health changes
        Health targetHealth = target.GetComponent<Health>();

        if (targetHealth != null)
        {
            targetHealth.Heal(healthToAdd, target.GetComponent<Pawn>());
        }

        // add score to the player
        Controller controller = target.GetComponentInChildren<Controller>();
        if (controller != null)
        {
            controller.score += scoreToAdd;
            controller.UpdateScore();
        }
    }

    public override void Remove(PowerupManager target)
    {
        // remove health changes
    }
}

