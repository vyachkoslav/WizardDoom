using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private Renderer objectRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;

    void Start()
    {
        objectRenderer = GetComponentInChildren<Renderer>();
        originalColor = objectRenderer.material.color;
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
