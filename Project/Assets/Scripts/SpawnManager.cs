using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] carPrefabs;
    public GameObject[] carPrefabs2;
    public GameObject[] powerupPrefabs;

    private GameManager gameManagerScript;
    private PlayerController playerControllerScript;

    public bool popVehicles;
    public bool popPowerups = true;

    public void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        if (popVehicles)
        {
            StartAllCars();
        }

        if (popPowerups)
        {
            StartCoroutine(nameof(SpawnRandomPowerups));//, new Vector2(-3, 3));
        }
    }

    public void StartAllCars()
    {

        float x = -12;
        float y = 3;
        float z = -25;

        float x2 = -7.5f;
        float y2 = 3;
        float z2 = 97;

        float range1 = 3.0f;
        float range2 = 5.0f;
        for (int i = 0; i < 5; i++)
        {
            SetCars(x, y, z, x2, y2, z2, range1, range2, i);
            x = x - 86f;
            x2 = x2 - 85.75f;
            range1 -= 0.25f;
            range2 -= 0.15f;
        }
    }

    public void SetCars(float x, float y, float z, float x2, float y2, float z2, float rangeOne, float rangeTwo, int avoid)
    {
        for (int i = 0; i < 6; i++)
        {
            if (avoid == 2 && i == 2)
            {
                Debug.Log("That was the line to avoid");
            }

            else
            {
                StartCoroutine(SpawnRandomCars(x, y, z, Random.Range(rangeOne, rangeTwo), 1));
                StartCoroutine(SpawnRandomCars(x2, y2, z2, Random.Range(rangeOne, rangeTwo), 2));
            }

            x = x - 11.4f;
            x2 = x2 - 11.3f;
        }
    }

    // Start is called before the first frame update
    public void StartGame()
    {
        playerControllerScript.SetFoxState();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerScript.gameOver == true)
        {
            GameObject[] cars = GameObject.FindGameObjectsWithTag("Cars");
            foreach (GameObject car in cars)
            {
                Destroy(car);
            }
        }

    }


    IEnumerator SpawnRandomCars(float x, float y, float z, float repeatRate, int direction)
    {
        while (playerControllerScript.gameOver == false)
        {
            int carIndex = Random.Range(0, carPrefabs.Length);
            Vector3 spawnPos = new Vector3(x, y, z);

            if (direction == 1)
            {
                Instantiate(carPrefabs[carIndex], spawnPos,
                carPrefabs[carIndex].transform.rotation);
            }
            else if (direction == 2)
            {
                Instantiate(carPrefabs2[carIndex], spawnPos,
                carPrefabs2[carIndex].transform.rotation);
            }


            yield return new WaitForSeconds(repeatRate);
        }
    }

    void SpawnRandomPowerups()
    {
        var playerController = GameObject.Find("Player").GetComponent<PlayerController>();

        var startZ = playerController.zRangeLow;
        var endZ = playerController.zRangeHigh;

        var levels = playerController.levelBounds;
        for (int i = 0; i < levels.Length; i++)
        {
            var levelBound = levels[i];
            var startX = levelBound.Start;
            var endX = levelBound.End;
            for (int j = 0; j < 3; j++)
            {
                int powerupIndex = Random.Range(0, powerupPrefabs.Length);
                Vector3 spawnPos = new Vector3(Random.Range(startX, endX), 3,
                    Random.Range(startZ, endZ));

                Instantiate(powerupPrefabs[powerupIndex], spawnPos,
                    powerupPrefabs[powerupIndex].transform.rotation);
            }
        }
    }
}
