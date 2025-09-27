using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private GameObject target;
    public float sightRange = 10f;
    public LayerMask playerLayerMask; // set this to PlayerMesh in the inspector

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        RaycastHit hit;
        bool rayCollided = Physics.Raycast(transform.position, transform.forward, out hit, sightRange, playerLayerMask);

        if (rayCollided)
        {
            Debug.Log("Ray hit entity");

            if (hit.collider.gameObject == target)
            {
                Debug.Log("Ray hit player");
            }
        }
        else
        {
            Debug.Log("Ray didn't hit anything");
        }
    }
}
