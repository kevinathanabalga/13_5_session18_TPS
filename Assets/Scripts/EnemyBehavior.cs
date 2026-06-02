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

    // Reference to Game Manager
    private GameBehavior _gameManager;

    // Damage cooldown
    private float _damageCooldown = 1f;
    private float _nextDamageTime = 0f;

    private bool _playerDetected = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            _player = playerObject.transform;
        }

        _gameManager = FindFirstObjectByType<GameBehavior>();

        if (PatrolRoute != null)
        {
            foreach (Transform child in PatrolRoute)
            {
                _locations.Add(child);
            }
        }

        if (_locations.Count > 0)
        {
            MoveToNextPatrolLocation();
        }
    }

    void Update()
    {
        if (_playerDetected && _player != null)
        {
            _agent.destination = _player.position;
        }
        else
        {
            if (_locations.Count > 0 &&
                !_agent.pathPending &&
                _agent.remainingDistance < 0.5f)
            {
                MoveToNextPatrolLocation();
            }
        }
    }

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
            _playerDetected = true;
            Debug.Log("Player detected - attack!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            _playerDetected = false;
            Debug.Log("Player out of range, resume patrol");

            if (_locations.Count > 0)
            {
                MoveToNextPatrolLocation();
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (Time.time >= _nextDamageTime)
            {
                if (_gameManager != null)
                {
                    _gameManager.HP -= 1;
                    Debug.Log("Player damaged!");
                }

                _nextDamageTime = Time.time + _damageCooldown;
            }
        }
    }
}