using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
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
    [SerializeField] private Vector3 myPosition;


    void Start()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        myPosition = transform.position;
        _timeToNextSpawn = spawnIntervalTime;
    }

    void Update()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        timer += Time.deltaTime;

        if (timer > _timeToNextSpawn)
        {
            if (propsToSpawn.Length <= 0) return;

            _timeToNextSpawn = timer + spawnIntervalTime;
            var pos = Random.insideUnitSphere * spawnRadius;
            pos.y = 0;
            pos += myPosition;

            // Spawn prop

            var prop = PhotonNetwork.InstantiateRoomObject(propsToSpawn[Random.Range(0, propsToSpawn.Length)].name, pos,
                quaternion.identity);
            prop.AddComponent<PropDestroyer>();
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