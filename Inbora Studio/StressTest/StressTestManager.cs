using UnityEngine;
using System.Collections.Generic;

public class StressTestManager : MonoBehaviour
{
    [Header("Cube Spawning Settings")]
    public int cubesPerFrame = 10;
    public Vector3 spawnArea = new Vector3(50, 50, 50);
    public GameObject cubePrefab;

    private bool stressTestActive = true;

    void Start()
    {
        if (cubePrefab == null)
        {
            cubePrefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cubePrefab.SetActive(false);
        }
    }

    void Update()
    {
        if (stressTestActive)
        {
            for (int i = 0; i < cubesPerFrame; i++)
            {
                SpawnRandomCube();
            }
        }
    }

    void SpawnRandomCube()
    {
        Vector3 pos = new Vector3(
            Random.Range(-spawnArea.x / 2, spawnArea.x / 2),
            Random.Range(0, spawnArea.y),
            Random.Range(-spawnArea.z / 2, spawnArea.z / 2)
        );
        GameObject cube = Instantiate(cubePrefab, pos, Random.rotation);
        cube.SetActive(true);
        cube.GetComponent<Renderer>().material.color = Random.ColorHSV();
        cube.AddComponent<Rigidbody>();
        Destroy(cube, 5f); // Destroy after 5 seconds
    }

    public void ToggleStressTest()
    {
        stressTestActive = !stressTestActive;
    }
} 