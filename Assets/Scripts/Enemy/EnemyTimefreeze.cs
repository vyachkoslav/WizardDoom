using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;

public class EnemyTimefreeze : MonoBehaviour
{
    private BaseEnemyAI _enemyAI;
    private DetectPlayer _detection;
    private NavMeshAgent _navMeshAgent;
    private GameObject _model;
    private Animator _animator;
    private SkinnedMeshRenderer _renderer;

    public Material temporalMaterial;
    private List<Material> originalMaterials = new List<Material>();
    private SkinnedMeshRenderer[] renderers;
    public bool IsTimeFrozen { get; private set; }

    private void Start()
    {
        _enemyAI = GetComponent<BaseEnemyAI>();
        _detection = GetComponent<DetectPlayer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _model = transform.Find("Model").gameObject;
        _animator = _model.GetComponent<Animator>();

        renderers = _model.GetComponentsInChildren<SkinnedMeshRenderer>();
        StoreOriginalMaterials();
        IsTimeFrozen = false;
    }

    public void ApplyTimefreeze(float duration)
    {
        if (!IsTimeFrozen)
        {
            StartCoroutine(TimefreezeCoroutine(duration));
        }
    }

    private IEnumerator TimefreezeCoroutine(float duration)
    {
        IsTimeFrozen = true;
        ApplyTemporalMaterial();

        _enemyAI.enabled = false;
        _detection.enabled = false;
        _navMeshAgent.enabled = false;
        _animator.enabled = false;

        yield return new WaitForSeconds(duration);
        
        _enemyAI.enabled = true;
        _detection.enabled = true;
        _navMeshAgent.enabled = true;
        _animator.enabled = true;

        RestoreOriginalMaterials();
        IsTimeFrozen = false;
    }

    public void StoreOriginalMaterials()
    {
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            originalMaterials.Add(renderer.material); ;
        }
    }

    public void ApplyTemporalMaterial()
    {
        foreach (SkinnedMeshRenderer renderer in renderers)
        {
            renderer.material = temporalMaterial;
        }
    }

    public void RestoreOriginalMaterials()
    {
        for (int i = 0; i < originalMaterials.Count; i++)
        {
            Material mat = originalMaterials[i];
            renderers[i].material = mat;
        }
    }
}
