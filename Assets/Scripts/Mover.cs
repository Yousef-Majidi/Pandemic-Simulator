using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();

    private Transform targetWaypoint;

    private int targetIndex = 0;

    private float minDist = 0.5f;

    private int lastIndex;

    private float moveSpeed = 5.0f;
    private float rotateSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        lastIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetIndex];
    }

    // Update is called once per frame
    void Update()
    {


        Quaternion rotationTarget = Quaternion.LookRotation(targetWaypoint.position - transform.position);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, rotateSpeed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);
        checkDistance(distance);
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, moveSpeed * Time.deltaTime);
    }

    void checkDistance(float currDist)
    {
        if(currDist < minDist)
        {
            targetIndex++;
            UpdateTarget();
        }
    }

    void UpdateTarget()
    {
        if(targetIndex > lastIndex)
        {
            targetIndex = 0;
        }
        targetWaypoint = waypoints[targetIndex];
    }

}
