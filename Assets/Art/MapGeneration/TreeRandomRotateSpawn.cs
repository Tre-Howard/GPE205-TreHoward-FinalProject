using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomRotateSpawn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f); // picks random y rotation when spawning
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
