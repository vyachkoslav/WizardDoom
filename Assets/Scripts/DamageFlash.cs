using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Renderer objectRenderer;
    private Entity entityScript;
    private bool takingDamage;

    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        entityScript = GetComponentInParent<Entity>();
        originalColor = objectRenderer.material.color;
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
        objectRenderer.material.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        objectRenderer.material.color = originalColor;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }
}
