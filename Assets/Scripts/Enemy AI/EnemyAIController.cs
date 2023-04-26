using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIController : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nav;

    [Tooltip("Starting position of the enemy")] [SerializeField] Vector3 _startingPos;

    [Tooltip("The different states")][SerializeField] enum State
    {
        Idle,
        Wandering,
        Targeting,
        Attacking
    }

    [Tooltip("The current State of the enemy")] [SerializeField] State _currentState;

    [Space(10)]
    [Header("Wandering")]
    [Tooltip("Range for a new position")] [SerializeField] int _wanderRadius = 10;
    [Tooltip("Distance that the enemy can wander from the starting position")] [SerializeField] int _maxWanderRadius = 20;
    [Tooltip("Next position for the enemy")] [SerializeField] Vector3 _walkPoint;
    [Tooltip("A check to see if the enemy can get a new position")] [SerializeField] bool _canGetNewPos;

    [Space(10)]
    [Header("Rotation")]
    [Tooltip("Speed of the rotation")] [SerializeField] float _rotationSpeed;
    [Tooltip("Rotational angle to look to the target")] [SerializeField] Quaternion _lookRotation;
    [Tooltip("Vector between current position and target")] [SerializeField] Vector3 _direction;

    [Space(10)]
    [Header("Player Targetting")]
    [Tooltip("Gets the position of the player")] [SerializeField] List<GameObject> _player;
    [Tooltip("List of Player GameObjects")] [SerializeField] List<Vector3> _playerPositions;
    [Tooltip("ID of the player being checked/targetted")] [SerializeField] int _playerIndex = -1;

    // Start is called before the first frame update
    void Awake()
    {
        _nav = GetComponent<NavMeshAgent>();
        _startingPos = transform.position;
        _walkPoint = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        _canGetNewPos = true;
        _currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //empties the positions of the players to replace them
        _playerPositions.Clear();

        //gets all of the players currently in game
        var _players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in _players)
        {
            if (!_player.Contains(player.gameObject))
            {
                Debug.Log("New Player!");
                _player.Add(player.gameObject);
            }

            Vector3 _position = player.transform.position;
            _playerPositions.Add(new Vector3(_position.x, _position.y, _position.z));
        }




        //gets result of whether player can be targeted, and sets the state according to this
        if(_currentState == State.Attacking)
        {
            AttackPlayer();
        }
        else if (CanTarget() && _currentState != State.Targeting)
        {
            _currentState = State.Targeting;
            MoveToPlayer();
        }
        
        else if (Random.Range(0, 10) < 3)
        {
            _currentState = State.Wandering;

            if (_canGetNewPos)
            {
                _canGetNewPos = false;
                StartCoroutine(NewPosition(false, false));
            }
        }
    }

    void Move()
    {
        float _distance = Vector3.Distance(transform.position, _walkPoint);

        if (_distance > 0.5f)
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
    public IEnumerator NewPosition(bool _quickChange, bool returnToStart)
    {
        if (!_quickChange)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        }

        if (!returnToStart)
        {
            Vector3 _randomDirection = Random.insideUnitSphere * _wanderRadius;

            //checks to see if the new position is within the max wandering distance from the starting position
            if (Vector3.Distance(_randomDirection, _startingPos) > _maxWanderRadius)
            {
                //sets quickChange to true so that the enemy doesnt wait longer than normal to get a new position
                StartCoroutine(NewPosition(true, false));
            }

            _randomDirection += transform.position;
            NavMeshHit _hit;
            NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
            _walkPoint = _hit.position;
        }

        Move();
        _canGetNewPos = true;
    }

    public bool CanTarget()
    {
        float distance = -1;

        //runs through all the players and gets the closest one
        foreach (Vector3 playerPos in _playerPositions)
        {
            if (Vector3.Distance(transform.position, playerPos) < distance || distance == -1)
            {
                distance = Vector3.Distance(transform.position, playerPos);
                _playerIndex = _playerPositions.IndexOf(playerPos);
            }
        }

        //check to see if the player can be seen and is within targetting range
        if (_playerIndex != -1 && distance < 20)
        {
            Debug.Log(gameObject.name + ": Can target player, " + distance + " from player");  
            return true;
        }
        else
        {
            Debug.Log(gameObject.name + ": Can't target player" + distance + " from player");
            //Debug.Log("distance greater than 20");
            return false;
        }
    }

    void MoveToPlayer()
    {
        _walkPoint = _playerPositions[_playerIndex];
        
        _nav.destination = _walkPoint;

        if (Vector3.Distance(transform.position, _walkPoint) < 3)
        {
            _nav.isStopped = true;
            _currentState = State.Attacking;
        }
        else
        {
            _nav.isStopped = false;
        }
    }

    void AttackPlayer()
    {
        Debug.Log(_player[_playerIndex].name + " is being attacked!");
    }
}