using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform mapCenter;
    public float maxDistance = 10f;

    private void Update()
    {
        //agent.Move()
    }

    private Vector3 GetRandomDestination(Transform center)
    {
        NavMeshHit hit;
        bool hasPosition = NavMesh.SamplePosition(Random.insideUnitSphere * maxDistance + center.position, out hit, maxDistance, 1);
        return hasPosition ? hit.position : center.position;
    }
}
