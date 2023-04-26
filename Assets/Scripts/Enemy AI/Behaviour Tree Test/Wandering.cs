using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;

public class Wandering : Node
{
    private Transform _transform;
    private NavMeshAgent _nav;

    Vector3 _startingPos;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    int _wanderRadius = 10;
    int _maxWanderRadius = 20;
    Vector3 _walkPoint;
    bool _canGetNewPos = true;

    float _rotationSpeed;
    Quaternion _lookRotation;
    Vector3 _direction;

    public Wandering(Transform transform, NavMeshAgent nav, Vector3 startingPos)
    {
        _transform = transform;
        _nav = nav;
        _startingPos = startingPos;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(_canGetNewPos);

        if (_canGetNewPos) {
            _canGetNewPos = false;
            Vector3 _randomDirection = Random.insideUnitSphere * _wanderRadius;

            //checks to see if the new position is within the max wandering distance from the starting position
            if (Vector3.Distance(_randomDirection, _startingPos) > _maxWanderRadius)
            {
                //sets quickChange to true so that the enemy doesnt wait longer than normal to get a new position
                Evaluate();
            }

            _randomDirection += _transform.position;
            NavMeshHit _hit;
            NavMesh.SamplePosition(_randomDirection, out _hit, _wanderRadius, 1);
            _walkPoint = _hit.position;

            float _distance = Vector3.Distance(_transform.position, _walkPoint);

            if (_direction != Vector3.zero)
            {
                //gets the vector pointing from the enemy's position to the target
                _direction = (_walkPoint - _transform.position).normalized;

                //gets the rotation the enemy needs to be in to look at the target
                _lookRotation = Quaternion.LookRotation(_direction);

                //rotates the enemy over time according to speed until it is in the corrent orientation
                _transform.rotation = Quaternion.Slerp(_transform.rotation, _lookRotation, Time.deltaTime * _rotationSpeed);

            //Sets the target point
            _nav.SetDestination(_walkPoint);
            }
        }

        if(Vector3.Distance(_transform.position, _walkPoint) < 0.1f)
        {
            _canGetNewPos = true;
        }

        state = NodeState.Running;
        return state;
    }

    //https://medium.com/geekculture/how-to-create-a-simple-behaviour-tree-in-unity-c-3964c84c060e
}
