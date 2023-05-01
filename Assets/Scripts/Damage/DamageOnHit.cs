using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DamageOnHit : MonoBehaviour
{

    public float damageDone;
    public Pawn owner;
    public string targetTag;


    public AudioSource audioSource;
    public AudioClip audioClipExplosion;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other) // object has this component, counts as a collider event and will trigger below
    {
        Health otherHealth = other.gameObject.GetComponent<Health>();
        // gets health component from gameobject this interacts with (collider overlapping)  

        // if you dont want to use tags, copy/paste all of this into another script, and remove the && other.CompareTag portion
        if (otherHealth != null) //only apply damage if other gameobject has health component and tag is player
        {
            otherHealth.TakeDamage(damageDone, owner);
            owner = null;
        }



        /*
        // spawn a new GameObject at the collision point
        GameObject soundObject = new GameObject();
        soundObject.transform.position = other.transform.position;
        Vector3 soundObjectPosition = soundObject.transform.position;

        
        // add an AudioSource to the new GameObject and play the audio clip
        AudioSource soundSource = soundObject.AddComponent<AudioSource>();

        if (audioClipExplosion != null)
        {
            soundSource.clip = audioClipExplosion;
            soundSource.spatialBlend = 1;
            AudioSource.PlayClipAtPoint(audioClipExplosion, soundObjectPosition);

            //soundSource.clip = audioClipExplosion;
            //AudioSource.PlayClipAtPoint(audioClipExplosion, soundObjectPosition);
        }
        else
        {
            Debug.LogError("Audio clip not set for DamageOnHit component");
        }

        // destroy the new GameObject after the audio clip has finished playing
        Destroy(soundObject, 5f); */

        // destroy self
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Pawn pawn = collision.gameObject.GetComponent<Pawn>();
        if (pawn != null && owner != pawn)
        {
            owner = pawn;
        }
    }
}

