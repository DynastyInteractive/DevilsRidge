using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    EnemyController _enemyController;
    [SerializeField] NavMeshAgent _nav;

    [Tooltip("Starting position of the enemy")] [SerializeField] Vector3 _startingPos;

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

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _nav = GetComponent<NavMeshAgent>();

        _nav.speed = _enemyController.Enemy.movementSpeed;
        _startingPos = transform.position;
        _walkPoint = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        _canGetNewPos = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_enemyController.State == SOEnemy.State.Idle)
        {
            _walkPoint = transform.position;
            Move();
        }

        if(_enemyController.State == SOEnemy.State.Wandering)
        {
            if (_canGetNewPos)
            {
                _canGetNewPos = false;
                StartCoroutine(NewPosition(false, false));
            }
        }
        else if(_enemyController.State == SOEnemy.State.Targeting)
        {
            StartCoroutine(NewPosition(false, false));
        }

        if(Vector3.Distance(transform.position, _startingPos) > _maxWanderRadius || (Vector3.Distance(transform.position, _walkPoint) < 0.5f))
        {
            _walkPoint = transform.position;
            _enemyController.CurrentlyMoving = false;
            _canGetNewPos = true;
        }
    }

    //Sets the new position when wandering
    //quickChange determines whether or not the enemy has to wait before getting a new position
    public IEnumerator NewPosition(bool _quickChange, bool returnToStart)
    {
        /*if (targetPlayer)
        {
            _walkPoint = _enemyController.PlayerPositions[_enemyController.PlayerIndex];
        }*/

        if (!_quickChange)
        {
            yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        }

        if (!returnToStart)
        {
            _walkPoint = new Vector3(Random.Range(_startingPos.x - 10, _startingPos.x + 10), transform.position.y, Random.Range(_startingPos.z - 10, _startingPos.z + 10));

            /*Vector3 _randomDirection = _startingPos + (Random.insideUnitSphere * _wanderRadius);

            //_randomDirection += transform.position;
            //NavMesh.SamplePosition(_randomDirection, out NavMeshHit _hit, _wanderRadius, 1);
            _walkPoint = _randomDirection;

            _walkPoint.y = transform.position.y;*/
        }

        _enemyController.CurrentlyMoving = true;
        Move();
    }

    void Move()
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

}
