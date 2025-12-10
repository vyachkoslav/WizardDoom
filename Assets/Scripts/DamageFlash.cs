using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Renderer objectRenderer;
    private Entity entityScript;
    private EnemyTimefreeze enemyTimefreeze;
    private bool takingDamage;

    [SerializeField] private Material flashMat;
    [SerializeField] private float flashDuration = 0.1f;

    private Material originalMaterial;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        entityScript = GetComponentInParent<Entity>();
        enemyTimefreeze = GetComponentInParent<EnemyTimefreeze>();
        originalMaterial = objectRenderer.material;
    }

    private void Update()
    {
        takingDamage = entityScript.takingDamage;
        if (takingDamage)
        {
            Flash();
        }
    }

    private IEnumerator DoFlash()
    {
        // Normal behaviour if temporal material is not applied
        if (!enemyTimefreeze.IsTimeFrozen)
        {
            objectRenderer.material = flashMat;
            yield return new WaitForSeconds(flashDuration);
            objectRenderer.material = originalMaterial;
        }
        else
        {
            objectRenderer.material = flashMat;
            yield return new WaitForSeconds(flashDuration);

            // Finally checks if time freeze effect is still active
            if (enemyTimefreeze.IsTimeFrozen)
            {
                objectRenderer.material = enemyTimefreeze.temporalMaterial;
            } 
        }
    }

    public void Flash()
    {
        StartCoroutine(DoFlash());
    }
}
