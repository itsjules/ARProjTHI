using UnityEngine;

public class ParticleEffectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ParticleEffect
    {
        public string effectName;
        public GameObject particlePrefab;
    }

    public ParticleEffect[] particleEffects;
    public float spawnDistance = -1f;  
    public float scaleMultiplier = 0.1f;

    public void SpawnEffect(string effectName)
    {
        foreach (var effect in particleEffects)
        {
            if (effect.effectName == effectName && effect.particlePrefab != null)
            {
                Vector3 spawnPosition = Camera.main.transform.position + Camera.main.transform.forward * spawnDistance;
                GameObject spawnedEffect = Instantiate(effect.particlePrefab, spawnPosition, Quaternion.identity);
                spawnedEffect.transform.localScale *= scaleMultiplier;
            }
            else{
                Debug.LogWarning("Particle effect not found: " + effectName);
            }
        }
        
    }
}

