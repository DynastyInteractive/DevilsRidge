using System.Collections;
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


    // Start is called before the first frame update
    void Start()
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
        if (_currentState == State.Wandering || (Random.Range(0, 10) < 3 && _currentState == State.Idle))
        {
            _currentState = State.Wandering;

            if (_newPos)
            {
                _newPos = false;
                StartCoroutine(NewPosition(false));
            }
        }
    }

    void Wander()
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



    public IEnumerator NewPosition(bool _quickChange) //quickChange determines whether or not the enemy has to wait before getting a new position
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

        Wander();
        _newPos = true;
    }
}
