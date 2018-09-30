using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public List<Tile> path;
    public int currentWaypoint = 0;
    private Tile targetWaypoint;

    public float moveSpeed = 5f;

    private void Start()
    {
        Vector3 firstPos = new Vector3(path[1].Position().x + 0.5f, 0, path[1].Position().y + 0.5f);
        transform.rotation = Quaternion.LookRotation(firstPos - transform.position);
    }

    private void Update()
    {
        if (currentWaypoint < path.Count)
        {
            if (targetWaypoint == null)
                targetWaypoint = path[0];
            Walk();
        }
    }

    private void Walk()
    {
        Vector3 nextWaypoint = new Vector3(targetWaypoint.X + 0.5f, 0, targetWaypoint.Y + 0.5f);

        // Rotate towards the waypoint;
        transform.forward = Vector3.RotateTowards(transform.forward, nextWaypoint - transform.position,
            moveSpeed * Time.deltaTime, 0.0f);

        // move towards the targetWaypoint
        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, moveSpeed * Time.deltaTime);

        if (transform.position == new Vector3(targetWaypoint.X, 0, targetWaypoint.Y))
        {
            currentWaypoint++;
            if (currentWaypoint == path.Count)
            {
                path = null;
                Destroy(gameObject);
            }
            else
            {
                targetWaypoint = path[currentWaypoint];
            }
        }
    }
}