using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour
{
    [SerializeField] protected Transform gunBulletSpawnPoint;
    
    public virtual void OnShoot()
    {
        //Shoot a bullet
    }
}
