using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MagicWandRuler : MonoBehaviour
{
    public void OnDrawGizmos()
    {
        int maxLength = 40;

        Gizmos.color = Color.green;

        Vector3 point = new Vector3(transform.position.x, 0, transform.position.z);


        Gizmos.DrawLine(new Vector3(transform.position.x, 40, transform.position.z), 
                        new Vector3(transform.position.x, -40, transform.position.z));
        Gizmos.color = Color.magenta;

        Gizmos.DrawLine(point + Vector3.back*3, point + Vector3.forward*3);
        Gizmos.DrawLine(point + Vector3.right*3, point + Vector3.left*3);
        Gizmos.DrawLine(point + Vector3.right * 3, point + Vector3.forward * 3);
        Gizmos.DrawLine(point + Vector3.right * 3, point + Vector3.back * 3);
        Gizmos.DrawLine(point + Vector3.left * 3, point + Vector3.forward * 3);
        Gizmos.DrawLine(point + Vector3.left * 3, point + Vector3.back * 3);
        
        Gizmos.color = Color.green;

        for (int i = -maxLength;i < maxLength;i++)
        {
            point = new Vector3(transform.position.x, 1 * i, transform.position.z);

            Gizmos.DrawLine(point + Vector3.back * 0.5f, point + Vector3.forward * 0.5f);
            Gizmos.DrawLine(point + Vector3.right * 0.5f, point + Vector3.left * 0.5f);

        }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(transform.position.x -10, transform.position.y, transform.position.z),
                new Vector3(transform.position.x + 10, transform.position.y, transform.position.z));
        for (int i = -10; i < 10; i++)
        {
            point = new Vector3(transform.position.x + 1*i, transform.position.y, transform.position.z);

            Gizmos.DrawLine(point + Vector3.back * 0.5f, point + Vector3.forward * 0.5f);
            Gizmos.DrawLine(point + Vector3.up * 0.5f, point + Vector3.down * 0.5f);

        }

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z - 10),
                new Vector3(transform.position.x, transform.position.y, transform.position.z + 10));
        for (int i = -10; i < 10; i++)
        {
            point = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1 * i);

            Gizmos.DrawLine(point + Vector3.right * 0.5f, point + Vector3.left * 0.5f);
            Gizmos.DrawLine(point + Vector3.up * 0.5f, point + Vector3.down * 0.5f);

        }
    }
}
