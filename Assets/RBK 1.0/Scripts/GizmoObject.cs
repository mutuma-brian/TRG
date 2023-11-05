using UnityEngine;
using System.Collections;

public class GizmoObject : MonoBehaviour {


    public Color GColor = Color.white;
    public float GSize = 1.0f;

    void OnDrawGizmos()
    {

        Gizmos.color = GColor;

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 2.0f;

        Gizmos.DrawRay(transform.position, direction);

        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one * GSize);
        Gizmos.matrix = rotationMatrix;

        Gizmos.DrawCube(Vector3.zero, Vector3.one * GSize);


    }


}
