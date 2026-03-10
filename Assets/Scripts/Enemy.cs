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
        agent = gameObject.GetComponent<NavMeshAgent>();
        setRandomSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && agent.isActiveAndEnabled)
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

        agent.enabled = false;
        int randomIndex = Random.Range(0, spawnPoints.transform.childCount);
        Debug.Log("Rand index: " + randomIndex);
        Transform randomSpawnPoint = spawnPoints.transform.GetChild(randomIndex);

        if (agent != null)
        {
            agent.velocity = Vector3.zero;
            Debug.Log("Velocity 0 ");
        }
        transform.localPosition = randomSpawnPoint.localPosition;
        if (agent != null)
        {
            agent.velocity = Vector3.zero;
            Debug.Log("Velocity 0 ");

        }
        Debug.Log("Pos: " + transform.localPosition);
        agent.enabled = true;
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
