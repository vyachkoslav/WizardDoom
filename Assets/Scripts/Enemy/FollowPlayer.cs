using UnityEngine;
using UnityEngine.AI;

public class FollowTarget : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    private Transform target;
    private string selfType;

    void Start()
    {
        // Gets the nav mesh agent of the object this script is attached to
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        selfType = gameObject.tag;
    }

    void Update()
    {
        // Fetches whether player is detecteed from DetectPlayer script
        if (GetComponent<DetectPlayer>().playerIsDetected)
        {
            if (selfType == "EnemyMelee")
            {
                // Sets nav mesh destination to player's position
                target = player.transform;
                agent.SetDestination(target.position);
            }
            
        }
        
    }
}
