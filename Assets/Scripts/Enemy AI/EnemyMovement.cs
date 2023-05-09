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

    #region Camps
    [SerializeField] List<GameObject> _camps;
    [SerializeField] GameObject _nearestCamp;
    EnemySpawner _spawner;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        _enemyController = GetComponent<EnemyController>();
        _nav = GetComponent<NavMeshAgent>();

        _nav.speed = _enemyController.Enemy.movementSpeed;
        _startingPos = transform.position;
        _walkPoint = new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z);
        _canGetNewPos = true;

        _nearestCamp = FindNearestCamp();
        _spawner = _nearestCamp.GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        _nav.isStopped= false;

        if(_enemyController.State == SOEnemy.State.Idle)
        {
            //_walkPoint = transform.position;
        }
        else if(_enemyController.State == SOEnemy.State.Wandering)
        {
            if (_canGetNewPos)
            {
                _canGetNewPos = false;
                //StartCoroutine(NewPosition(false, false));
                NewPosition(false);
            }

            //Debug.Log(Vector3.Distance(transform.position, _walkPoint));
            if ((Vector3.Distance(transform.position, _walkPoint) < 0.5f) && _enemyController.State == SOEnemy.State.Wandering)
            {
                Debug.Log("New Position");
                _enemyController.CurrentlyMoving = false;
                _canGetNewPos = true;
            }
        }
        else if(_enemyController.State == SOEnemy.State.Targeting)
        {
            _walkPoint = _enemyController.PlayerPositions[_enemyController.PlayerIndex];
            StartCoroutine(Move(true));
        }
        else
        {
            _nav.isStopped = true;
        }
    }

    public GameObject FindNearestCamp()
    {
        GameObject[] camps;
        camps = GameObject.FindGameObjectsWithTag("EnemyCamp");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject camp in camps)
        {
            Vector3 difference = camp.transform.position - position;
            float sqrDistance = difference.sqrMagnitude;

            if (sqrDistance < distance)
            {
                closest = camp;
                distance = sqrDistance;
            }
        }
        return closest;
    }

    //Sets the new position when wandering
    //quickChange determines whether or not the enemy has to wait before getting a new position
    public void NewPosition(bool returnToStart)
    {
        _enemyController.CurrentlyMoving = true;

        if (!returnToStart)
        {

            /*float randX = Random.Range(_spawner.transform.position.x - _spawner.CampRadius, _spawner.transform.position.x + _spawner.CampRadius);
            float randY = Random.Range(_spawner.transform.position.z - _spawner.CampRadius, _spawner.transform.position.z + _spawner.CampRadius);
            //Debug.Log("New Pos");
            _walkPoint = new Vector3(randX, transform.position.y, randY);*/

            Vector3 _randomDirection = _nearestCamp.transform.position + (Random.insideUnitSphere * _spawner.CampRadius);

            //_randomDirection += transform.position;
            NavMesh.SamplePosition(_randomDirection, out NavMeshHit _hit, _spawner.CampRadius, 1);
            _walkPoint = _randomDirection;

            _walkPoint.y = transform.position.y;
        }

        StartCoroutine(Move(false));
    }

    IEnumerator Move(bool _quickChange)
    {
        if (!_quickChange)  yield return new WaitForSeconds(Random.Range(2.5f, 4f));
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, _walkPoint);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _enemyController.Enemy.targetRange);
    }
}
