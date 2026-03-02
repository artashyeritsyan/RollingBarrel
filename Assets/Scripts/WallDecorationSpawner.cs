using UnityEngine;

public class WallDecorationSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] wallSpawnPoints;
    [SerializeField] GameObject[] wallDecorations;
    [SerializeField] GameObject[] groundSpawnPoints;
    [SerializeField] GameObject[] groundDecorations;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Haskanal vocna aveli chisht, amen angam nor decoratioin object sarqel heto jnjel
    //       te 2 hat object mejy pahel u erb vor petqa meky miacnel myusy anjatel ??
    public void GenerateWallDecorations()
    {
        for (int i = 0; i < wallSpawnPoints.Length; i++)
        {
            Transform spawnPoint = wallSpawnPoints[i].transform;

            if (spawnPoint.childCount > 0)
            {
                for (int j = spawnPoint.childCount - 1; j >= 0; j--)
                {
                    Destroy(spawnPoint.GetChild(j).gameObject);
                }
            }   

            int randomNumber = Random.Range(0, wallDecorations.Length * 2);
            if (randomNumber < wallDecorations.Length)
            {
                GameObject newObj = Instantiate(wallDecorations[randomNumber],
                wallSpawnPoints[i].transform.position, wallSpawnPoints[i].transform.rotation);

                newObj.transform.SetParent(wallSpawnPoints[i].transform);

                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
            }

        }
    }

    public void GenerateGroundDecorations()
    {
        for (int i = 0; i < groundSpawnPoints.Length; i++)
        {
            Transform spawnPoint = groundSpawnPoints[i].transform;

            if (spawnPoint.childCount > 0)
            {
                for (int j = spawnPoint.childCount - 1; j >= 0; j--)
                {
                    Destroy(spawnPoint.GetChild(j).gameObject);
                }
            }

            int randomNumber = Random.Range(0, groundDecorations.Length * 2);
            if (randomNumber < groundDecorations.Length)
            {
                GameObject newObj = Instantiate(groundDecorations[randomNumber],
                groundSpawnPoints[i].transform.position, groundSpawnPoints[i].transform.rotation);

                newObj.transform.SetParent(groundSpawnPoints[i].transform);

                newObj.transform.localPosition = Vector3.zero;
                newObj.transform.localRotation = Quaternion.identity;
            }

        }
    }

}
