using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] private float waypointSize = 1f;

    public Transform GetNextWaypoint(Transform currWay)
    {
        if(currWay == null)
        {
            return transform.GetChild(0);
        }

        if(currWay.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(transform.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
    }
   
}
