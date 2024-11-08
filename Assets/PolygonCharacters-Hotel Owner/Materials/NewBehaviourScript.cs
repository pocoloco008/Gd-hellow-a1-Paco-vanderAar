using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement3D : MonoBehaviour
{
    public float moveSpeed = 3f;       // Snelheid van het object
    public float rotationSpeed = 200f; // Rotatiesnelheid van het object
    public float moveRadius = 5f;      // Straal voor willekeurige beweging
    public float waitTime = 2f;        // Wachttijd tussen bewegingen

    private Vector3 targetPosition;    // Doelpositie van het object in 3D
    private float waitTimer;
    private bool isRotating = false;   // Controleert of het object bezig is met draaien

    void Start()
    {
        SetNewRandomTarget();
        waitTimer = waitTime;
    }

    void Update()
    {
        if (isRotating)
        {
            RotateTowardsTarget();
        }
        else
        {
            // Beweeg het object naar de doelpositie als de rotatie voltooid is
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Controleer of het object de doelpositie heeft bereikt
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                waitTimer -= Time.deltaTime;

                // Als de wachttijd voorbij is, stel een nieuwe doelpositie in
                if (waitTimer <= 0f)
                {
                    SetNewRandomTarget();
                    waitTimer = waitTime;
                }
            }
        }
    }

    void SetNewRandomTarget()
    {
        // Kies een willekeurige X- en Z-positie binnen de straal rondom de huidige positie
        float randomX = Random.Range(-moveRadius, moveRadius) + transform.position.x;
        float randomZ = Random.Range(-moveRadius, moveRadius) + transform.position.z;
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        // Start rotatie naar de nieuwe doelpositie
        isRotating = true;
    }

    void RotateTowardsTarget()
    {
        // Bereken de richting naar de doelpositie op het XZ-vlak
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Voer de rotatie uit richting het doel (alleen om de Y-as)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Controleer of de rotatie voltooid is
        if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
        {
            isRotating = false;
        }
    }
}