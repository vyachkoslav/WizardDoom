using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public bool IsInAttackRange = false;
    public float rangeSize;
    SphereCollider attackRangeCollider;

    void Start()
    {
        attackRangeCollider = GetComponent<SphereCollider>();
        rangeSize = attackRangeCollider.radius * transform.localScale.x;
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInAttackRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsInAttackRange = false;
        }
    }
}
