using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ring : MonoBehaviour
{
    private Transform player;
    public GameObject[] childRings;
    float radius = 100f;
    float force = 500f;

    // Event to notify when the ring is passed
    public delegate void RingPassed();
    public event RingPassed OnRingPassed;

    private GameManager gameManager;  // Reference to GameManager

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = FindObjectOfType<GameManager>();  // Get the GameManager reference
    }

    private void Update()
    {
        if (transform.position.y > player.position.y)
        {
            // Update the number of passing rings and score
            GameManager.noOfPassingRings++;

            // Notify that the player has passed the ring
            OnRingPassed?.Invoke();

            // Update score in the GameManager
            gameManager.AddScore(1);  // Add 1 point per successful pass

            // Play sound effect
            FindObjectOfType<AudioManager>().Play("Whoosh");

            for (int i = 0; i < childRings.Length; i++)
            {
                childRings[i].GetComponent<Rigidbody>().isKinematic = false;
                childRings[i].GetComponent<Rigidbody>().useGravity = true;

                Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

                foreach (Collider newCollider in colliders)
                {
                    Rigidbody rb = newCollider.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.AddExplosionForce(force, transform.position, radius);
                    }
                }

                childRings[i].GetComponent<MeshCollider>().enabled = false;
                childRings[i].transform.parent = null;
                Destroy(childRings[i].gameObject, 2f);
                Destroy(this.gameObject, 5f);
            }

            this.enabled = false;  // Disable this script once the ring is passed
        }
    }
}
