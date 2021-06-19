using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour
{
    Transform[] spawnPoints;
    List<Vector3> spawnPositions = new List<Vector3>();

    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    public void ResetSpawnPoints()
    {
        spawnPositions.Clear();

        foreach (Transform t in spawnPoints)
        {
            spawnPositions.Add(t.position);
        }
    }

    public Vector3 GetRandomSpawnPosition()
    {
        if (spawnPositions.Count == 0)
        {
            Debug.LogError("SpawnPoints: no more positions to assign");
            return Vector3.zero;
        }

        Vector3 position = spawnPositions[Random.Range(0, spawnPositions.Count)];
        spawnPositions.Remove(position);

        return position;
    }
}