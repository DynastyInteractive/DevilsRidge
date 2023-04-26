using System.Collections;
using System.Collections.Generic;
using BehaviourTree;

public class Enemy : Tree
{
    public UnityEngine.AI.NavMeshAgent _nav;

    [UnityEngine.Space(10)]
    [UnityEngine.Tooltip("Gets the position of the player")] [UnityEngine.SerializeField] List<UnityEngine.GameObject> _player;
    [UnityEngine.Tooltip("List of Player GameObjects")] [UnityEngine.SerializeField] List<UnityEngine.Vector3> _playerPositions;
    [UnityEngine.Tooltip("ID of the player being checked/targetted")] [UnityEngine.SerializeField] int _playerIndex = -1;

    private void Update()
    {
        //empties the positions of the players to replace them
        _playerPositions.Clear();

        //gets all of the players currently in game
        var _players = FindObjectsOfType<PlayerMovement>();
        foreach (PlayerMovement player in _players)
        {
            if (!_player.Contains(player.gameObject))
            {
                UnityEngine.Debug.Log("New Player!");
                _player.Add(player.gameObject);
            }

            UnityEngine.Vector3 _position = player.transform.position;
            _playerPositions.Add(new UnityEngine.Vector3(_position.x, _position.y, _position.z));
        }
    }

    protected override Node SetupTree()
    {
        if (transform != null && _nav != null)
        {
            Node root = new Wandering(transform, _nav);
            return root;
        }

        return null;
    }
}
