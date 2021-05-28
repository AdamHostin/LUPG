using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathGizmo : MonoBehaviour
{

    [SerializeField] Color color;
    [SerializeField] float sphereRadius;

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
            Gizmos.DrawWireSphere(transform.GetChild(i).position, sphereRadius);
        }


        Gizmos.DrawWireSphere(transform.GetChild((transform.childCount - 1)).position, sphereRadius);
    }
}

