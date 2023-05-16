using System.Collections;
using UnityEngine;

public class SpawnNextNPC : MonoBehaviour
{
    [SerializeField] DialogueSequencer _dialogueSequencer;
    [SerializeField] GameObject _nextNPC;
    [SerializeField] Vector3 _spawnPos;

    // Start is called before the first frame update
    void Start()
    {
        _dialogueSequencer.OnDialogueEnded += SpawnNPC;
    }

    void SpawnNPC()
    {
        UIManager.Instance.StartCoroutine(SpawnDelayed());
        _dialogueSequencer.OnDialogueEnded -= SpawnNPC;
        Destroy(gameObject);

        IEnumerator SpawnDelayed()
        {
            yield return new WaitForSeconds(0.5f);
            Instantiate(_nextNPC, _spawnPos, Quaternion.identity);
        }
    }
}
