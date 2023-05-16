using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : NetworkBehaviour
{
    EnemyController _enemyController;
    [SerializeField] NavMeshAgent _nav;

    [Tooltip("Starting position of the enemy")] [SerializeField] Vector3 _startingPos;

    [Space(10)]
    [Header("Wandering")]
    [Tooltip("Range for a new position")] [SerializeField] int _wanderRadius = 10;
    [Tooltip("Distance that the enemy can wander from the starting position")] [SerializeField] int _maxWanderRadius = 20;
    [Tooltip("Next position for the enemy")] [SerializeField] Vector3 _walkPoint;

    [Space(10)]
    [Header("Rotation")]
    [Tooltip("Speed of the rotation")] [SerializeField] float _rotationSpeed;
    [Tooltip("Rotational angle to look to the target")] [SerializeField] Quaternion _lookRotation;
    [Tooltip("Vector between current position and target")] [SerializeField] Vector3 _direction;

    [SerializeField] bool isTargeting;

    #region Camps
    EnemySpawner _spawner;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (!IsHost) return;

        _enemyController = GetComponent<EnemyController>();
        _nav = GetComponent<NavMeshAgent>();

        _nav.speed = _enemyController.Enemy._agility.BaseValue;
        _startingPos = transform.position;
        _walkPoint = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);

        _spawner = _enemyController.NearestCamp.GetComponent<EnemySpawner>();

        isTargeting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost) return;

        //if(_enemyController.Enemy.isBoss) Debug.Log(_enemyController.Enemy._agility);
        if (_enemyController.State == SOEnemy.State.Idle)
        {
            _nav.speed = 0;
        }
        else if(_enemyController.State == SOEnemy.State.Wandering)
        {
            _nav.speed = _enemyController.Enemy._agility.BaseValue;
            if (isTargeting && (_enemyController.Enemy.isBoss || Vector3.Distance(transform.position, _enemyController.NearestCamp.transform.position) > _enemyController.NearestCamp.GetComponent<EnemySpawner>().CampRadius * 2))
            {
                isTargeting = false;
                NewPosition(true);
            }

            if (_enemyController.CanGetNewPosition)
            {
                _enemyController.CanGetNewPosition = false;
                NewPosition(false);
            }
        }
        else if(_enemyController.State == SOEnemy.State.Targeting)
        {
            _nav.speed = _enemyController.Enemy._agility.BaseValue;
            isTargeting = true;
            _walkPoint = _enemyController.PlayerPositions[_enemyController.ClosestPlayerIndex];
            StartCoroutine(Move(true));
        }
        else
        {
            _nav.speed = 0;
        }
        
        if ((Vector3.Distance(transform.position, _walkPoint) < 0.5f))
        {
            //Debug.Log("New Position");
            _enemyController.IsCurrentlyMoving = false;
            _enemyController.CanGetNewPosition = true;
        }

        _enemyController.Animator?.SetFloat("Speed", _nav.speed);
    }

    //Sets the new position when wandering
    //quickChange determines whether or not the enemy has to wait before getting a new position
    public void NewPosition(bool returnToStart)
    {
        if (!IsHost) return;

        bool quickChange;

        if (!returnToStart)
        {
            /*float randX = Random.Range(_spawner.transform.position.x - _spawner.CampRadius, _spawner.transform.position.x + _spawner.CampRadius);
            float randY = Random.Range(_spawner.transform.position.z - _spawner.CampRadius, _spawner.transform.position.z + _spawner.CampRadius);
            //Debug.Log("New Pos");
            _walkPoint = new Vector3(randX, transform.position.y, randY);*/

            float radius;
            if (_enemyController.Enemy.isBoss) radius = _spawner.CampRadius / 2;
            else radius = _spawner.CampRadius;

            Vector3 _randomDirection = _enemyController.NearestCamp.transform.position + (Random.insideUnitSphere * radius);

            //_randomDirection += transform.position;
            NavMesh.SamplePosition(_randomDirection, out NavMeshHit _hit, _spawner.CampRadius, 1);
            _walkPoint = _randomDirection;
            quickChange = false;
        }
        else
        {
            _walkPoint = _startingPos;
            quickChange = true;
        }
        _walkPoint.y = transform.position.y;

        StartCoroutine(Move(quickChange));
    }

    IEnumerator Move(bool _quickChange)
    {
        if (!IsHost) yield return null;

        if (!_quickChange)  yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        _enemyController.IsCurrentlyMoving = true;
        
        if (_direction != Vector3.zero)
        {
            LookAt(_walkPoint);
        }

        //Sets the target point
        _nav.SetDestination(_walkPoint);
    }

    public void LookAt(Vector3 dir)
    {
        if (!IsHost) return;

        //gets the vector pointing from the enemy's position to the target
        _direction = (dir - transform.position).normalized;

        //gets the rotation the enemy needs to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotates the enemy over time according to speed until it is in the corrent orientation
        transform.localRotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _walkPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyController.Enemy.targetRange);
    }
}
