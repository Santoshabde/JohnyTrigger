using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SNGames.JonnyTriggerProto
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private float bulletSpeed;

        private void Start()
        {
            Destroy(this.gameObject, 10f);
        }

        private void Update()
        {
            transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collided with: " + other.name);
            IDamagable damagableComponent = other.GetComponentInParent<IDamagable>();
            if (damagableComponent != null)
            {
                damagableComponent.OnDamage(other.GetComponent<Rigidbody>(), 0.01f, transform.forward);
                Destroy(this.gameObject);
            }
        }
    }
}