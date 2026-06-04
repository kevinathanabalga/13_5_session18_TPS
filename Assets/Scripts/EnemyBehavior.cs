using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    // Enemy HP
    public int MaxHP = 3;
    private int _currentHP;

    // Detection
    public float DetectionRange = 10f;
    private bool _playerDetected = false;

    // NavMesh Agent
    private NavMeshAgent _agent;

    // Patrol Route
    public Transform PatrolRoute;
    private List<Transform> _locations = new List<Transform>();
    private int _locationIndex = 0;

    // References
    private Transform _player;
    private GameBehavior _gameManager;

    // Damage Cooldown
    private float _damageCooldown = 1f;
    private float _nextDamageTime = 0f;

    void Start()
    {
        _currentHP = MaxHP;

        _agent = GetComponent<NavMeshAgent>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

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
        if (_player == null || _agent == null)
            return;

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                _player.position
            );

        _playerDetected =
            distanceToPlayer <= DetectionRange;

        if (_playerDetected)
        {
            _agent.destination =
                _player.position;
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

        _agent.destination =
            _locations[_locationIndex].position;

        _locationIndex =
            (_locationIndex + 1) %
            _locations.Count;
    }

    public void TakeDamage(int damage)
    {
        _currentHP -= damage;

        Debug.Log(
            gameObject.name +
            " HP: " +
            _currentHP +
            "/" +
            MaxHP
        );

        if (_currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " defeated!");
        Destroy(gameObject);
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time >= _nextDamageTime)
            {
                if (_gameManager != null)
                {
                    _gameManager.HP -= 1;
                    Debug.Log("Player damaged!");
                }

                _nextDamageTime =
                    Time.time +
                    _damageCooldown;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            DetectionRange
        );
    }
}