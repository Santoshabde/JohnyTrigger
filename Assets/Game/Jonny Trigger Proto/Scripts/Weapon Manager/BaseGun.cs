using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class BaseGun : MonoBehaviour
    {
        [SerializeField] protected Transform gunBulletSpawnPoint;
        [SerializeField] protected BulletController bullet;

        public virtual void OnShoot(Vector3 bulletShotDirection)
        {
            //Shoot a bullet
            GameObject bulletGO = Instantiate(bullet.gameObject, gunBulletSpawnPoint.position, gunBulletSpawnPoint.rotation);
            bulletGO.transform.forward = bulletShotDirection;
        }
    }
}