using UnityEngine;
using System.Collections;

public class CenterAxle : MonoBehaviour {


    public Transform PointA;
    public Transform PointB;

	void Update () {

        Vector3 v = new Vector3(0, 0, 0); // Choose this as you wish
        Vector3 AB = PointB.position - PointA.position;
        Vector3 C_prime = PointA.position + AB / 2;
        Vector3 C = C_prime + Vector3.Normalize(Vector3.Cross(AB, v));

        Vector3 targetDir = PointA.localPosition - PointB.localPosition;
        Vector3 forward = -PointA.up;
        float angle = Vector3.Angle(targetDir, -Vector3.up);

        transform.position = C;
        transform.localEulerAngles = new Vector3(0, 0, angle-90);

      
	}
}
