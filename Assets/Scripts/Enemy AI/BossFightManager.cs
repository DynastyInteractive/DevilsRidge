using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossFightManager : MonoBehaviour
{
    EnemySpawner enemySpawner;

    [SerializeField] bool isFightActive; //checks to see if the fight is active
    [SerializeField] bool isInCamp; //checks to see if any players are inside the camp area
    [SerializeField] bool isInRange; //checks to see if the player is in range to keep the fight active
    [SerializeField] List<GameObject> players = new List<GameObject>();

    [SerializeField] GameObject _bossFightUI;
    [SerializeField] Slider _slider;

    public bool IsFightActive
    {
        get { return isFightActive; }
    }

    public List<GameObject> Players
    {
        get { return players; }
    }

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        isFightActive = false;
        _bossFightUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if fight is active and player is within campRadius +10, continue fight. if player is out of range, reset boss
        isInRange = false;
        var _players = FindObjectsOfType<Player>();
        foreach(Player player in _players)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < enemySpawner.CampRadius + 10)
            {
                isInRange = true;
            }
        }

        if(isFightActive && isInRange)
        {
            //do fight
            _bossFightUI.SetActive(true);
        }
        else
        {
            isFightActive = false;
            _bossFightUI.SetActive(false);
            //reset
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Start Boss Fight");
            Debug.Log(other.gameObject.name);
            isFightActive = true;
            isInCamp = true;
            //start boss fight
            //set fight start to true
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            isInCamp = false;
        }
    }

    public void SetMaxHealth(float health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(float health)
    {
        _slider.value = health;
    }
}
