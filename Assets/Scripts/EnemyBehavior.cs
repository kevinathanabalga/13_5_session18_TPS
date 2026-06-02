using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // NavMesh Agent
    private NavMeshAgent _agent;

    // Patrol route
    public Transform PatrolRoute;
    private List<Transform> _locations = new List<Transform>();
    private int _locationIndex = 0;

    // Reference to Player
    private Transform _player;

    void Start()
    {
        // Setup NavMesh Agent
        _agent = GetComponent<NavMeshAgent>();

        // Find Player
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }

        // Setup patrol route
        if (PatrolRoute != null)
        {
            foreach (Transform child in PatrolRoute)
            {
                _locations.Add(child);
            }
        }

        // Start patrol
        if (_locations.Count > 0)
        {
            MoveToNextPatrolLocation();
        }
    }

    void Update()
    {
        // Check if enemy reached current waypoint
        if (_locations.Count > 0 &&
            _agent != null &&
            _agent.remainingDistance < 0.5f &&
            !_agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
    }

    // Move to next waypoint
    void MoveToNextPatrolLocation()
    {
        if (_locations.Count == 0 || _agent == null)
            return;

        _agent.destination = _locations[_locationIndex].position;
        _locationIndex = (_locationIndex + 1) % _locations.Count;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player detected - attack!");

            // Chase player
            if (_agent != null && _player != null)
            {
                _agent.destination = _player.position;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Debug.Log("Player out of range, resume patrol");

            // Resume patrol
            if (_locations.Count > 0)
            {
                MoveToNextPatrolLocation();
            }
        }
    }
}