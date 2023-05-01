using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public List<Powerup> powerups;
    private List<Powerup> removedPowerupQueue;

    // Start is called before the first frame update
    void Start()
    {
        powerups = new List<Powerup>();
        removedPowerupQueue = new List<Powerup>();
    }

    // Update is called once per frame
    void Update()
    {
        DecrementPowerupTimers();
    }

    private void LateUpdate()
    {
        ApplyRemovePowerupsQueue();
    }

    public void Add(Powerup powerupToAdd)
    {
        // add() method
        powerupToAdd.Apply(this);
        powerups.Add(powerupToAdd);
    }

    public void Remove(Powerup powerupToRemove)
    {
        // remove() method
        powerupToRemove.Remove(this); // casts to Powerup script where you enact the remove function (in class, it would be Remove() from HealthPowerup)
        // you can mouse of Remove above to see where it comes from too since we are sharing the word Remove

        removedPowerupQueue.Add(powerupToRemove); // adding to removed queue
    }

    public void DecrementPowerupTimers()
    {
        //one at a time, put object in powerups list and loop
        foreach (Powerup powerup in powerups)
        {
            powerup.duration -= Time.deltaTime; // subtract by frame draw

            if (powerup.duration <= 0)
            {
                Remove(powerup);
            }
        }
    }

    private void ApplyRemovePowerupsQueue()
    {
        if (powerups != null)
        {
            // Remove powerups from our temporary list
            foreach (Powerup powerup in removedPowerupQueue)
            {
                if (powerups.Contains(powerup))
                {
                    powerups.Remove(powerup);
                }
            }
            removedPowerupQueue.Clear();
        }
        else
        {
            Debug.LogWarning("ApplyRemovePowerupQueue null exception");
        }
    }
}
