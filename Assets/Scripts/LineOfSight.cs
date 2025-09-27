using UnityEngine;

public class LineOfSight : MonoBehaviour
{
    private GameObject target;
    public float sightRange = 10f;
    public float fieldOfViewAngle = 180f;
    public LayerMask playerLayerMask; // set this to PlayerMesh in the inspector
    public LayerMask obstacleLayerMask; // set this to the layer of environmental objects

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        Vector3 directionToPlayer = target.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= sightRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < fieldOfViewAngle/2)
            {
                RaycastHit hit;
                bool rayCollided = Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightRange, playerLayerMask | obstacleLayerMask);

                if (rayCollided)
                {
                    if (hit.collider.gameObject == target)
                    {
                        Debug.DrawLine(transform.position, hit.collider.transform.position, Color.green);
                        //Debug.Log("Ray hit player");
                    }
                    else
                    {
                        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                        //Debug.Log("Ray hit an obstacle");
                    }
                }
                
            }
            else
            {
                Debug.Log("Player is outside vision cone");
            }
        }
        else
        {
            Debug.DrawLine(transform.position, transform.forward * sightRange, Color.red);
            Debug.Log("Player is out of sight range");
        }

        
    }
}
