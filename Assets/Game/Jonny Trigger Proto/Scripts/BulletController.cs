using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;

        private void Update()
        {
            transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamagable damagableComponent = other.GetComponentInParent<IDamagable>();
            if (damagableComponent != null)
            {
                damagableComponent.OnDamage(other.GetComponent<Rigidbody>(), 10f, transform.forward);
            }
        }
    }
}