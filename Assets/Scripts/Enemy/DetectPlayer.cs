using System;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private GameObject target;
    public bool playerIsDetected = false;
    public float sightRange = 10f;
    public float fieldOfViewAngle = 180f;
    public float proximityRange = 4f;
    private float distanceToPlayer;
    public LayerMask playerLayerMask; // set this to PlayerMesh in the inspector
    public LayerMask obstacleLayerMask; // set this to the layer of environmental objects

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // If either method returns true, player is detected
        if (ProximityDetection() || LineOfSightDetection())
        {
            playerIsDetected = true;
        }
        else
        {
            playerIsDetected = false;
        }

        // Pass this on to movement script, just to get rid of no-use-warning for now
        bool Detection = playerIsDetected;
    }

    bool ProximityDetection()
    {
        bool playerIsNearby = false;
        distanceToPlayer = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToPlayer < proximityRange)
        {
            playerIsNearby = true;
            //Debug.Log("Player is nearby");
        }

        return playerIsNearby;
    }

    bool LineOfSightDetection()
    {
        bool playerIsSeen = false;

        Vector3 directionToPlayer = target.transform.position - transform.position;
        distanceToPlayer = directionToPlayer.magnitude;

        // Checks to see if player is in enemy sight range
        if (distanceToPlayer <= sightRange)
        {
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            // Checks if player is within either half of vision cone
            if (angleToPlayer < fieldOfViewAngle / 2)
            {
                RaycastHit hit;
                bool rayCollided = Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, sightRange,
                // Layer masks work with bits so using bitwise OR (|) operator
                playerLayerMask | obstacleLayerMask);

                if (rayCollided)
                {
                    if (hit.collider.gameObject == target)
                    {
                        playerIsSeen = true;
                        Debug.DrawLine(transform.position, hit.collider.transform.position, Color.green);
                        //Debug.Log("Ray hit player");
                    }
                    else
                    {
                        playerIsSeen = false;
                        Debug.DrawLine(transform.position, hit.transform.position, Color.red);
                        //Debug.Log("Ray hit an obstacle");
                    }
                }

            }
            else
            {
                playerIsSeen = false;
                //Debug.Log("Player is outside vision cone");
            }
        }
        else
        {
            playerIsSeen = false;
            Debug.DrawLine(transform.position, transform.forward * sightRange, Color.red);
            //Debug.Log("Player is out of sight range");
        }

        return playerIsSeen;
    }
}
