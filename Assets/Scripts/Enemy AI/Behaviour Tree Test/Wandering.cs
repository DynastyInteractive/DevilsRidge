using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using BehaviourTree;

public class Wandering : Node
{
    [SerializeField] Transform _transform;
    [SerializeField] UnityEngine.AI.NavMeshAgent _nav;

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

    public Wandering(Transform transform, UnityEngine.AI.NavMeshAgent nav)
    {
        _transform = transform;
        _nav = nav;
    }

    void Start()
    {
        _startingPos = _transform.position;
        _walkPoint = new Vector3(_transform.position.x, _transform.position.y - 0.4f, _transform.position.z);
        _canGetNewPos = true;
    }

    public override NodeState Evaluate()
    {
        StartCoroutine(NewPosition(true, false));

        _state = NodeState.Running;
        return _state;
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

            _randomDirection += _transform.position;
            UnityEngine.AI.NavMeshHit _hit;
            UnityEngine.AI.NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
            _walkPoint = _hit.position;
        }

        Move();
        _canGetNewPos = true;
    }

    private void StartCoroutine(IEnumerator enumerator)
    {
        throw new System.NotImplementedException();
    }

    void Move()
    {
        float _distance = Vector3.Distance(_transform.position, _walkPoint);

        if (_distance > 0.5f)
        {
            if (_direction != Vector3.zero)
            {
                //gets the vector pointing from the enemy's position to the target
                _direction = (_walkPoint - _transform.position).normalized;

                //gets the rotation the enemy needs to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotates the enemy over time according to speed until it is in the corrent orientation
                _transform.rotation = Quaternion.Slerp(_transform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed);
            }

            //Sets the target point
            _nav.SetDestination(_walkPoint);
        }
    }

    //https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
}
