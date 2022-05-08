using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using PlayerScripts;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class PropSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfProps = 20;
    [SerializeField] private GameObject[] propsToSpawn;
    [SerializeField] private float spawnRadius = 18f;
    [SerializeField] private float spawnIntervalTime = 10f;
    [SerializeField] private float _timeToNextSpawn;
    [SerializeField] private List<GameObject> spawnedProps = new List<GameObject>();
    [SerializeField] private float timer;

    void Start()
    {
        for (int i = 0; i < numberOfProps; i++)
        {
            SpawnInCircle();
        }

        _timeToNextSpawn = spawnIntervalTime;
    }

    private void SpawnInCircle()
    {
        // Set new prop position
        var myPosition = transform.position;
        var pos = Random.insideUnitSphere * spawnRadius;
        pos.y = 0;
        pos += myPosition;

        // Spawn prop
        var prop = Instantiate(propsToSpawn[Random.Range(0, propsToSpawn.Length)], pos, quaternion.identity);
        spawnedProps.Add(prop);
        prop.AddComponent<PropDestroyer>();
        prop.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > _timeToNextSpawn)
        {
            if (propsToSpawn.Length <= 0) return;
            
            _timeToNextSpawn = timer + spawnIntervalTime;
            spawnedProps[0].SetActive(true);
            spawnedProps.RemoveAt(0);
        }
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Tan(ang * Mathf.Deg2Rad);
        return pos;
    }
}