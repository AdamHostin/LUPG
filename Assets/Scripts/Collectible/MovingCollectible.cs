using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCollectible : MonoBehaviour
{
    [SerializeField] Transform path;
    [SerializeField] float speed;
    [SerializeField] float offsetToRegisterWaypoint;

    [Header("Dont touch")]
    [SerializeField] CollectibleSinMovement collectible;

    private Transform[] waypoints;
    private int currentWaypointIndex = 1;
    private bool isMovingForward = true;


    // Start is called before the first frame update
    void Awake()
    {
        InitWaypoints();
    }

    void InitWaypoints()
    {
        if (path.childCount < 2)
        {
            Debug.LogError("Not enough waypoints. Path needs to have at least 2 waypoints");
            this.enabled = false;
        }

        waypoints = new Transform[path.childCount];
        for (int i = 0; i < path.childCount; i++)
        {
            waypoints[i] = path.GetChild(i);
        }
    }

    private void OnEnable()
    {
        if (Random.Range(0f, 1f) < 0.5)
        {
            transform.position = waypoints[0].position;
            currentWaypointIndex = 1;
        }
        else
        {
            transform.position = waypoints[waypoints.Length-1].position;
            currentWaypointIndex = waypoints.Length - 2;
        }        
    }

    int GetNextWaypointIndex()
    {
        if (isMovingForward)
        {
            if (currentWaypointIndex + 1 < waypoints.Length) return currentWaypointIndex + 1;

            isMovingForward = false;
            collectible.ResetTimer();
            return GetNextWaypointIndex();
        
        }
        else
        {
            if (currentWaypointIndex - 1 >= 0) return currentWaypointIndex - 1;

            isMovingForward = true;
            collectible.ResetTimer();
            return GetNextWaypointIndex();
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position,waypoints[currentWaypointIndex].position) < offsetToRegisterWaypoint)
        {
            currentWaypointIndex = GetNextWaypointIndex();
        }

        transform.Translate((waypoints[currentWaypointIndex].position - transform.position).normalized * speed * Time.deltaTime);
    }
}
