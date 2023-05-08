using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] SOEnemy _enemy;

    [Tooltip("The current State of the enemy")] [SerializeField] SOEnemy.State _currentState;

    [SerializeField] bool _currentlyMoving;

    [Space(10)]
    [Header("Player Targetting")]
    [Tooltip("List of Player GameObjects")] [SerializeField] List<Vector3> _playerPositions;
    [Tooltip("ID of the player being checked/targetted")] [SerializeField] int _playerIndex = -1;

    public SOEnemy Enemy
    {
        get { return _enemy; }
    }

    public SOEnemy.State State
    {
        get { return _currentState; }
    }

    public bool CurrentlyMoving
    {
        get { return _currentlyMoving; }
        set { _currentlyMoving = value; }
    }

    public List<Vector3> PlayerPositions
    {
        get { return _playerPositions; }
    }

    public int PlayerIndex
    {
        get { return _playerIndex; }
    }

    // Start is called before the first frame update
    void Start()
    {
        _currentState = SOEnemy.State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        //gets the result of checking what state the enemy is in, and sets it to the current state variable
        _currentState = DetermineState();

        //empties the positions of the players to replace them
        _playerPositions.Clear();

        //gets all of the players currently in game
        var _players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in _players)
        {
            Vector3 _position = player.transform.position;
            _playerPositions.Add(new Vector3(_position.x, _position.y, _position.z));
        }

        if (_playerIndex != -1) Debug.Log(_playerPositions[_playerIndex]);
    }

    //returns the current state, depending on its current situation
    SOEnemy.State DetermineState()
    {
        if (CanTarget())
        {
            if (Vector3.Distance(transform.position, _playerPositions[_playerIndex]) < _enemy.attackRange)
            {
                return SOEnemy.State.Attacking;
            }
            else return SOEnemy.State.Targeting;
        }
        else if (Random.Range(0, 5) == 0 || CurrentlyMoving)
        {
            return SOEnemy.State.Wandering;
        }

        return SOEnemy.State.Idle;
    }

    public bool CanTarget()
    {
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

        // Check to see if the closest player can be seen and is within targeting range
        if (closestPlayerIndex != -1 && closestPlayerDistance < _enemy.targetRange)
        {
            _playerIndex = closestPlayerIndex;
            return true;
        }
        else
        {
            return false;
        }
    }
}