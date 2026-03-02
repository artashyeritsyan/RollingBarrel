using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public Transform player;

    [SerializeField] GameObject spawnPoints;
    [SerializeField] Vector3 defaultPosition;
 
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
    }

    void ResetHandler()
    {
        setRandomSpawnPosition();
    }

    void setRandomSpawnPosition()
    {
        if (spawnPoints == null)
        {
            transform.position = defaultPosition;
        }


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
