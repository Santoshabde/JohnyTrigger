using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SNGames.CommonModule;

public class ParticleEffectsController : SerializeSingleton<ParticleEffectsController>
{
    [SerializeField] private List<ParticleEffectObject> particleEffects;

    public void SpawnParticleEffect(string particleID, Vector3 position, Quaternion rotation)
    {
        GameObject particleToSpawn = particleEffects.Find(t => t.particleID == particleID).particleEffect;

        if (particleToSpawn != null)
            Instantiate(particleToSpawn, position, rotation);
        else
            Debug.LogError("Mentioned particle id not present");
    }
}

[System.Serializable]
public struct ParticleEffectObject
{
    public string particleID;
    public GameObject particleEffect;
}
