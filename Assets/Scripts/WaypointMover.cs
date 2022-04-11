using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    //Stores reference to WaypointSystem
    [SerializeField] private Waypoints waypoints;
    [SerializeField] private Rigidbody2D rb2d;
    [SerializeField] private Frontline_Manager fManager;
    [SerializeField] private float multiplierSpeed = 5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool directionLeft, directionRight;
    [SerializeField] private float distanceThreshold = 0.1f;

    private Transform currentWaypoint;
    // Start is called before the first frame update
    void Start()
    {
        //Setting initial position
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //Set the next waypoint target
        currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
        
    }

    // Update is called once per frame
    void Update()
    {
        multiplierSpeed = fManager.multiplierSpeed;
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * multiplierSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentWaypoint.position) < distanceThreshold)
        {
            Debug.Log("Checkpoint 1");
            currentWaypoint = waypoints.GetNextWaypoint(currentWaypoint);
            if (directionLeft)
            {
                Debug.Log("Checkpoint 2");
                rb2d.SetRotation(rb2d.rotation + 90);
            }
            if (directionRight)
            {
                Debug.Log("Checkpoint 3");
                rb2d.SetRotation(rb2d.rotation - 90);
                
            }
        }
    }
}
