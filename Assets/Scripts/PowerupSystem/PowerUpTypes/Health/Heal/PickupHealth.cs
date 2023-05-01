using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickupHealth : MonoBehaviour
{
    public PowerupHealth powerup;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        if (powerupManager != null)
        {
            //add powerup
            powerupManager.Add(powerup);

            GameObject soundObject = new GameObject();
            soundObject.transform.position = transform.position;

            // add an AudioSource to the new GameObject and play the audio clip
            AudioSource soundSource = soundObject.AddComponent<AudioSource>();
            soundSource.clip = this.GetComponent<AudioSource>().clip;


            if (soundSource != null)
            {
                soundSource.Play();
            }
            else
            {
                Debug.LogError("Audio clip not set for DamageOnHit component");
            }

            // destroy the new GameObject after the audio clip has finished playing
            Destroy(soundObject, 5f);

            Destroy(gameObject);
        }
    }

    /*   public void OnTriggerEnter(Collider other)
       {
           PowerupManager powerupManager = other.GetComponent<PowerupManager>();

           if (powerupManager != null)
           {
               //add powerup
               powerupManager.Add(powerup);

               AudioSource audioSource = GetComponent<AudioSource>();
               if (audioSource != null)
               {
                   audioSource.Play();
               }

               Destroy(gameObject);
           }

       }*/
}
