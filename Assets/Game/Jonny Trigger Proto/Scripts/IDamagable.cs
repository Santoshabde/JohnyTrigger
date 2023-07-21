using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public void OnDamage(Rigidbody rigidBodyHit, float damageAmount, Vector3 damageDirection);
}
