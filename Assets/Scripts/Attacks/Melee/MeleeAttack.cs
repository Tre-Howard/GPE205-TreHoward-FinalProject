using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeAttack : MonoBehaviour
{
    public abstract void Start();
    public abstract void Update();
    public abstract void Melee(GameObject meleePrefab, float fireForce, float damageDone, float lifespan);
}
