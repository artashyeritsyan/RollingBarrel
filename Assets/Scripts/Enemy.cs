using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static System.Math;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;

    //[Header("Footsteps")]
    //public AudioSource walkSounds;
    //public List<AudioClip> footsteps;

    [Header("Spawn point")]
    [SerializeField] GameObject spawnPoints;

    void Start()
    {
        setRandomSpawnPosition();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.position);


        }

        //if (Abs(transform.position.x) - Abs(player.position.x) < 10 || Abs(transform.position.z) - Abs(player.position.z) < 10)
        //{
        //    walkSounds.Play();
        //    Debug.Log("SOUNDD");
        //}



    }



    void ResetHandler()
    {
        setRandomSpawnPosition();
    }

    void setRandomSpawnPosition()
    {
        if (spawnPoints == null) return;

        Debug.Log(spawnPoints.transform.childCount);

        int randomIndex = Random.Range(0, spawnPoints.transform.childCount); // max is exclusive
        Debug.Log("Rand index: " + randomIndex);
        Transform randomSpawnPoint = spawnPoints.transform.GetChild(randomIndex);

        transform.position = randomSpawnPoint.position;
    }

    void OnEnable()
    {
        MenuController.OnGameReset += ResetHandler;
    }

    void OnDisable()
    {
        MenuController.OnGameReset -= ResetHandler;
    }

}
