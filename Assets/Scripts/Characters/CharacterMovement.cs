using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    
    protected NavMeshAgent agent;

    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void ToggleMovement(bool enabled)
    {
        agent.enabled = enabled;
    }
    public bool IsMoving()
    {
        if(!agent.enabled) return false;
        float v = agent.velocity.sqrMagnitude;
        return v > 0;
    }
    public void MoveTo(NPCLocationState locationState)
    {
        Trasition_Scenes.Locations locationToMove = locationState.location;
        Trasition_Scenes.Locations currentLocation = Trasition_Scenes.instance.currentLocation;

        if(locationToMove == Trasition_Scenes.instance.currentLocation)
        {
            NavMeshHit hit;
            NavMesh.SamplePosition(locationState.coord,out hit,10f, NavMesh.AllAreas);
            if (Vector3.Distance(transform.position, hit.position) < 1) return;
            agent.SetDestination(hit.position);
            return;
        }

        Trasition_Scenes.Locations nextLocation = LocationManager.GetNextLocation(currentLocation, locationToMove);

        Vector3 destination = LocationManager.Instance.GetExitPosition(nextLocation).position;
        agent.SetDestination(destination);
    }
}
