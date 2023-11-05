using System.Collections;
using UnityEngine;

public class AIController : MonoBehaviour
{
    public float accel = 0.8f;        // Rate of acceleration.
    public float inertia = 0.9f;     // Amount of velocity retained during slowing down.
    public float speedLimit = 10.0f;  // Maximum speed.

    private float currentSpeed = 0.0f;  // Current speed.

    public float rotationDamping = 6.0f; // Rotation damping.
    public bool smoothRotation = true;  // Use smooth rotation.

    public Transform[] waypoints;       // Array of waypoints.
    private int WPindexPointer = 0;     // Index of the current waypoint.
    private Transform waypoint;          // Current waypoint.

    // Start is called before the first frame update
    void Start()
    {
        waypoint = waypoints[WPindexPointer];
    }

    // Update is called once per frame
    void Update()
    {
        Accell();
    }

    // Accel function
    void Accell()
    {
        if (waypoint)
        {
            if (smoothRotation)
            {
                Quaternion rotation = Quaternion.LookRotation(waypoint.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationDamping);
            }
        }

        currentSpeed = currentSpeed + accel * accel;
        transform.Translate(0, 0, Time.deltaTime * currentSpeed);

        if (currentSpeed >= speedLimit)
        {
            currentSpeed = speedLimit;
        }
    }

    // OnTriggerEnter is called when the GameObject collides with a trigger collider.
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint")) // Assuming waypoints have the "Waypoint" tag.
        {
            WPindexPointer++;

            if (WPindexPointer >= waypoints.Length)
            {
                WPindexPointer = 0; // Reset to the first waypoint to loop.
            }

            waypoint = waypoints[WPindexPointer];
        }
    }
}
