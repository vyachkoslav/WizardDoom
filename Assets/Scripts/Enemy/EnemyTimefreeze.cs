using System.Collections;
using UnityEngine;

public class EnemyTimefreeze : MonoBehaviour
{
    [SerializeField] BaseEnemyAI enemyAI;
    [SerializeField] DetectPlayer enemyDetection;

    public void ApplyTimefreeze(float duration)
    {
        StartCoroutine(TimefreezeCoroutine(duration));
    }

    private IEnumerator TimefreezeCoroutine(float duration)
    {
        enemyAI.enabled = false;
        enemyDetection.enabled = false;
        yield return new WaitForSeconds(duration);
        enemyAI.enabled = true;
        enemyDetection.enabled = true;
    }

}
