using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine.AI;

public class SimpleEnemyBehaviour : MonoBehaviour
{

    private NavMeshAgent agent;
    [SerializeField] Transform agentTarget;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(agentTarget.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
}
