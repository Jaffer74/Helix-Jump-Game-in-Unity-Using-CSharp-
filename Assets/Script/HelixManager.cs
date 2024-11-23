using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixManager : MonoBehaviour
{
    public GameObject[] rings; // Array of different ring prefabs
    public int noOfRings;
    public float ringDistance = 5f;
    float yPos;

    private void Start()
    {
        // Reset yPos before spawning rings
        yPos = 0f;

        // Adjust noOfRings based on level or some dynamic input
        noOfRings = GameManager.CurrentLevelIndex + 5; // Assuming GameManager keeps track of levels, increase rings with level

        for (int i = 0; i < noOfRings; i++)
        {
            if (i == 0)
            {
                SpawnRings(0); // First ring
            }
            else
            {
                SpawnRings(Random.Range(1, rings.Length - 1)); // Random ring between first and last
            }
        }
        SpawnRings(rings.Length - 1); // Last ring
    }

    void SpawnRings(int index)
    {
        // Instantiate the ring at the correct position
        GameObject newRing = Instantiate(rings[index], new Vector3(transform.position.x, yPos, transform.position.z), Quaternion.identity);
        yPos -= ringDistance; // Move down to place the next ring
        newRing.transform.parent = transform; // Set the parent for organization
    }
}
