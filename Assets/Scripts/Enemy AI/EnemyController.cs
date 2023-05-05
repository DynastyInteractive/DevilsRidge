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
    [Tooltip("Gets the position of the player")] [SerializeField] List<GameObject> _player;
    [Tooltip("List of Player GameObjects")] [SerializeField] List<Vector3> _playerPositions;
    [Tooltip("ID of the player being checked/targetted")] [SerializeField] int _playerIndex = -1;
    [Tooltip("Distance that the enemy can target the player")] [SerializeField] Vector3 _targetRange;

    [Space(10)]
    [Header("Attacking the player")]
    [Tooltip("Distance that the enemy can attack the player")] [SerializeField] Vector3 _attackingRange;

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
            if (!_player.Contains(player.gameObject))
            {
                Debug.Log("New Player!");
                _player.Add(player.gameObject);
            }

            Vector3 _position = player.transform.position;
            _playerPositions.Add(new Vector3(_position.x, _position.y, _position.z));
        }

        Debug.Log(_playerPositions[_playerIndex]);
    }

    //returns the current state, depending on its current situation
    SOEnemy.State DetermineState()
    {
        if (CanTarget())
        {
            if (Vector3.Distance(transform.position, _playerPositions[_playerIndex]) < 3)
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
            //Debug.Log(gameObject.name + ": Can target player, " + distance + " from player");
            return true;
        }
        else
        {
            //Debug.Log(gameObject.name + ": Can't target player" + distance + " from player");
            return false;
        }
    }
}