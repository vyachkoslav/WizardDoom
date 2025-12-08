using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTimefreeze : MonoBehaviour
{
    private BaseEnemyAI _enemyAI;
    private DetectPlayer _detection;
    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private void Start()
    {
        _enemyAI = GetComponent<BaseEnemyAI>();
        _detection = GetComponent<DetectPlayer>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = transform.Find("Model").GetComponent<Animator>();
    }

    public void ApplyTimefreeze(float duration)
    {
        StartCoroutine(TimefreezeCoroutine(duration));
    }

    private IEnumerator TimefreezeCoroutine(float duration)
    {
        _enemyAI.enabled = false;
        _detection.enabled = false;
        _navMeshAgent.enabled = false;
        _animator.enabled = false;

        yield return new WaitForSeconds(duration);
        
        _enemyAI.enabled = true;
        _detection.enabled = true;
        _navMeshAgent.enabled = true;
        _animator.enabled = true;
    }

}
