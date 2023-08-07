using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDamagable : MonoBehaviour, IDamagable
{
    [SerializeField] private float effectRadius;
    public void OnDamage(Rigidbody rigidBodyHit, float damageAmount, Vector3 damageDirection)
    {
        ParticleEffectsController.Instance.SpawnParticleEffect("ExplosionEffect1", transform.position, Quaternion.identity);
        transform.GetComponent<MeshRenderer>().enabled = false;
        transform.GetComponent<Collider>().enabled = false;
        Invoke("StartDamageEffect", 0.02f);
    }

    private void StartDamageEffect()
    {
        Collider[] collidersOverlapped = Physics.OverlapSphere(transform.position, effectRadius);
        foreach (var item in collidersOverlapped)
        {
            IDamagable damagableComponent = item.GetComponentInParent<IDamagable>();
            if (damagableComponent != null)
            {
                damagableComponent.OnDamage(item.GetComponent<Rigidbody>(), 2000f, (item.transform.position - transform.position).normalized);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red - new Color(0, 0, 0, 0.8f);
        Gizmos.DrawSphere(transform.position, effectRadius);
    }
}
