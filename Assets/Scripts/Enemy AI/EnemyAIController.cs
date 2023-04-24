using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nav;

    [Tooltip("Starting position of the enemy")][SerializeField] Vector3 _startingPos;

    [Tooltip("The different states")][SerializeField] enum State
    {
        Idle,
        Wandering,
        Targetting,
        Attacking
    }
    [Tooltip("The current State of the enemy")][SerializeField] State _currentState;

    [Space(10)]
    [Header("Wandering")]
    [Tooltip("Range for a new position")][SerializeField] int _wanderRadius = 10;
    [Tooltip("Distance that the enemy can wander from the starting position")][SerializeField] int _maxWanderRadius = 20;
    [Tooltip("Current position of the enemy")][SerializeField] Vector3 _currentPos;
    [Tooltip("Next position for the enemy")][SerializeField] Vector3 _walkPoint;
    [Tooltip("A check to see if the enemy can get a new position")][SerializeField] bool _newPos;

    [Space(10)]
    [Header("Rotation")]
    [Tooltip("Speed of the rotation")][SerializeField] float _rotationSpeed;
    [Tooltip("Rotational angle to look to the target")][SerializeField] Quaternion _lookRotation;
    [Tooltip("Vector between current position and target")][SerializeField] Vector3 _direction;

    [Space(10)]
    [Header("Player Targetting")]
    [Tooltip("Gets the position of the player")][SerializeField] List<GameObject> _player;
    [Tooltip("List of Player GameObjects")][SerializeField] List<Vector3> _playerTransforms;
    [Tooltip("ID of the player being checked/targetted")][SerializeField] int _playerID = -1;

    // Start is called before the first frame update
    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _startingPos = transform.position;
        _walkPoint = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        _newPos = true;
        _currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //empties the positions of the players to replace them
        _playerTransforms.Clear();

        //gets all of the players currently in game
        var _players = FindObjectsOfType<PlayerMovement>();
        foreach(PlayerMovement player in _players) 
        {
            if (!_player.Contains(player.gameObject))
            {
                Debug.Log("New Player!");
                _player.Add(player.gameObject);
            }

            Transform _position = player.gameObject.GetComponent<Transform>();
            _playerTransforms.Add(new Vector3(_position.position.x, _position.position.y, _position.position.z));
        }

        /*foreach(Vector3 player in _playerTransforms)
        {
            Debug.Log(player);
        }*/

        if (CanTarget() || _currentState == State.Wandering || (Random.Range(0, 10) < 3 && _currentState == State.Idle))
        {
            _currentState = State.Wandering;

            if (_newPos)
            {
                _newPos = false;
                StartCoroutine(NewPosition(false));
            }
        }
        else if (CanTarget())
        {
            _currentState = State.Targetting;
        }
    }

    void MoveTo()
    {
        _currentPos = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);

        float _distance = Vector3.Distance(_currentPos, _walkPoint);

        if (_distance > 0.5)
        {
            if (_direction != Vector3.zero)
            {
                //gets the vector pointing from the enemy's position to the target
                _direction = (_walkPoint - transform.position).normalized;

                //gets the rotation the enemy needs to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotates the enemy over time according to speed until it is in the corrent orientation
                transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed);
            }

            //Sets the target point
            _nav.SetDestination(_walkPoint);
        }
        else
        {
            _currentState = State.Idle;
        }
    }

    //Sets the new position when wandering
    //quickChange determines whether or not the enemy has to wait before getting a new position
    public IEnumerator NewPosition(bool _quickChange) 
    {
        if (!_quickChange)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        }

        Vector3 _randomDirection = Random.insideUnitSphere * _wanderRadius;

        //checks to see if the new position is within the max wandering distance from the starting position
        if (Vector3.Distance(_randomDirection, _startingPos) > _maxWanderRadius)
        {
            //sets quickChange to true so that the enemy doesnt wait longer than normal to get a new position
            StartCoroutine(NewPosition(true));
        }

        _randomDirection += transform.position;
        NavMeshHit _hit;
        NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
        _walkPoint = _hit.position;

        MoveTo();
        _newPos = true;
    }

    public bool CanTarget()
    {
        float _distance = 10000000000;
        
        //runs through all the players and gets the closest one
        foreach (Vector3 players in _playerTransforms)
        {
            if(Vector3.Distance(_currentPos, players) < _distance)
            {
                _distance = Vector3.Distance(_currentPos, players);
                _playerID = _playerTransforms.IndexOf(players);
            }
        }

        if (_playerID != -1)
        {
            Vector3 _playerPos = new Vector3(_playerTransforms[_playerID].x, _playerTransforms[_playerID].y, _playerTransforms[_playerID].z);
            RaycastHit _hit;
            if (Physics.Raycast(_currentPos, _playerPos, out _hit))
            {
                if (_hit.transform.name == _player[_playerID].name && _distance < 20)
                {
                    Debug.Log(gameObject.name + ": Can target player");
                    return true;
                }
            }
        }
        else
        {
            Debug.Log("No Player ID Found");
            return false;
        }

        Debug.Log(gameObject.name + ": Can't target player");
        return false;
    }

    void MoveToPlayer()
    {
        _walkPoint = _playerTransforms[_playerID];
        MoveTo();
    }
}
