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
            Vector3 spawnPoint = gunBulletSpawnPoint.position;
            spawnPoint.x = 0;

            GameObject bulletGO = Instantiate(bullet.gameObject, spawnPoint, Quaternion.identity);
            bulletGO.transform.forward = bulletShotDirection;
        }
    }
}