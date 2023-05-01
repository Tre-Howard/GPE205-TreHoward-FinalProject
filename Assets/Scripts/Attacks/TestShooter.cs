using System.Collections;
using UnityEngine;

public class TestShooter : Shooter
{
    public Transform[] firePoints;
    public GameObject bulletPrefab;

    public float fireForce;
    public float damageDone;
    public float bulletLifespan;

    private float timeUntilNextEvent;
    public float fireRate;

    private AudioSource audioSource;

    public override void Start()
    {
        timeUntilNextEvent = fireRate;
        audioSource = GetComponent<AudioSource>();
    }

    public override void Update()
    {
        timeUntilNextEvent -= Time.deltaTime;

        if (timeUntilNextEvent <= 0)
        {
            timeUntilNextEvent = fireRate;            
        }
    }

    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        targetPosition.y = 0f;

        Transform chosenFirePoint = ChooseFirePoint();

        GameObject newBullet = Instantiate(bulletPrefab, chosenFirePoint.position, Quaternion.identity);
        newBullet.transform.LookAt(targetPosition);

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(newBullet.transform.forward * fireForce, ForceMode.Impulse);
        }

        Destroy(newBullet, bulletLifespan);

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    /*    public override void Shoot(GameObject shellPrefab, float fireForce, float damageDone, float lifespan)
    {
        ShootCooldown();

        if (canShoot)
        {
            // Get the mouse position in screen coordinates
            Vector3 mousePosition = Input.mousePosition;

            // Convert the screen coordinates to world coordinates
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.transform.position.y));

            // Calculate the direction from the fire point to the mouse position
            Vector3 direction = worldMousePosition - firepointTransform.position;
            direction.y = 0f;
            direction.Normalize();

            // Spawn the shell prefab in the direction of the mouse click
            GameObject newShell = Instantiate(shellPrefab, firepointTransform.position + direction * 0.5f, Quaternion.LookRotation(direction)) as GameObject;
            Rigidbody rb = newShell.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddForce(direction * fireForce);
            }

            DamageOnHit doh = newShell.GetComponent<DamageOnHit>();
            if (doh != null)
            {
                doh.damageDone = damageDone;
                doh.owner = GetComponent<Pawn>();
            }

            canShoot = false;
            Destroy(newShell, lifespan);
        }
    }*/

    private Transform ChooseFirePoint()
    {
        int index = Random.Range(0, firePoints.Length);
        return firePoints[index];
    }
}