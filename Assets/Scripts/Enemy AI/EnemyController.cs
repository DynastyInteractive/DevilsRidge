using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemyController : NetworkBehaviour
{
    [SerializeField] SOEnemy _enemy;
    [SerializeField] Animator _animator;

    [Tooltip("The current State of the enemy")] [SerializeField] SOEnemy.State _currentState;

    [SerializeField] bool _isCurrentlyMoving;
    [Tooltip("A check to see if the enemy can get a new position")] [SerializeField] bool _canGetNewPosition;

    [Space(10)]
    [Header("Player Targetting")]
    [Tooltip("List of Player Positions")] [SerializeField] List<Vector3> _playerPositions;
    [Tooltip("List of Player GameObjects")] [SerializeField] List<GameObject> _playerObjs;
    [Tooltip("ID of the player being checked/targetted")] [SerializeField] int _closestPlayerIndex = -1;

    [SerializeField] GameObject _nearestCamp;

    public bool CanGetNewPosition
    {
        get { return _canGetNewPosition; }
        set { _canGetNewPosition = value;}
    }
    public Animator Animator
    {
        get { return _animator; }
    }
    public GameObject NearestCamp
    {
        get { return _nearestCamp; }
        set { _nearestCamp = value; }
    }

    public SOEnemy Enemy
    {
        get { return _enemy; }
    }

    public SOEnemy.State State
    {
        get { return _currentState; }
    }

    public bool IsCurrentlyMoving
    {
        get { return _isCurrentlyMoving; }
        set { _isCurrentlyMoving = value; }
    }

    public List<Vector3> PlayerPositions
    {
        get { return _playerPositions; }
    } 
    
    public List<GameObject> PlayerObjs
    {
        get { return _playerObjs; }
    }

    public int ClosestPlayerIndex
    {
        get { return _closestPlayerIndex; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentState = SOEnemy.State.Idle;
        _animator = GetComponent<Animator>();
        _canGetNewPosition = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) return;

        //gets the result of checking what state the enemy is in, and sets it to the current state variable
        if(_enemy.isBoss && _nearestCamp.GetComponent<BossFightManager>().IsFightActive) _currentState = DetermineState(true);
        else _currentState = DetermineState(false);

        //empties the positions of the players to replace them
        _playerPositions.Clear();
        _playerObjs.Clear();

        //gets all of the players currently in game
        var _players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in _players)
        {
            Vector3 _position = player.transform.position;
            _playerPositions.Add(new Vector3(_position.x, _position.y, _position.z));
            if (!_playerObjs.Contains(player.gameObject)) _playerObjs.Add(player.gameObject);
        }

        if (_closestPlayerIndex >= _playerPositions.Count) _closestPlayerIndex = -1;
    }

    //returns the current state, depending on its current situation
    SOEnemy.State DetermineState(bool isBossFight)
    {
        if (!IsHost) return SOEnemy.State.Idle;

        if (isBossFight)
        {
            foreach (GameObject player in _nearestCamp.GetComponent<BossFightManager>().Players) {
                if (_enemy.isBoss) _playerPositions.Add(new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z));
            }
        }

        if ((CanTarget() || isBossFight) && Vector3.Distance(transform.position, _nearestCamp.transform.position) < _nearestCamp.GetComponent<EnemySpawner>().CampRadius * 2)
        {
            if (Vector3.Distance(transform.position, _playerPositions[_closestPlayerIndex]) < _enemy.attackRange)
            {
                return SOEnemy.State.Attacking;
            }
            return SOEnemy.State.Targeting;
        }
        else if (IsCurrentlyMoving || (Random.Range(0, 5) == 0 && _canGetNewPosition))
        {
            return SOEnemy.State.Wandering;
        }

        return SOEnemy.State.Idle;
    }

    public bool CanTarget()
    {
        if (!IsHost) return false;

        float closestPlayerDistance = Mathf.Infinity;
        int closestPlayerIndex = -1;

        // Iterate through all the players and find the closest one
        for (int i = 0; i < _playerPositions.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, _playerPositions[i]);

            if (distance < closestPlayerDistance)
            {
                closestPlayerDistance = distance;
                closestPlayerIndex = i;
            }
        }

        _closestPlayerIndex = closestPlayerIndex;

        // Check to see if the closest player can be seen and is within targeting range
        if (closestPlayerIndex != -1 && closestPlayerDistance < _enemy.targetRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}