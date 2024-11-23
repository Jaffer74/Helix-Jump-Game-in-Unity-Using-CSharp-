using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody rb; // Define the Rigidbody variable
    public float bounceForce = 400f;
    new AudioManager audio; // Correctly define the AudioManager variable
    public GameObject splitPrefab;

    private void Start()
    {
        audio = FindObjectOfType<AudioManager>(); // Use 'audio' to find the AudioManager instance
        rb = GetComponent<Rigidbody>(); // Initialize the Rigidbody component
    }

    // This is the collision detection method
    private void OnCollisionEnter(Collision other)
    {
        rb.velocity = new Vector3(rb.velocity.x, bounceForce * Time.deltaTime, rb.velocity.z);
        audio.Play("Land"); // Play the sound when landing

        // Instantiate a split prefab at collision position
        GameObject newsplit = Instantiate(splitPrefab, new Vector3(transform.position.x, other.transform.position.y + 0.19f, transform.position.z), transform.rotation);
        newsplit.transform.localScale = Vector3.one * Random.Range(0.8f, 1.2f);
        newsplit.transform.parent = other.transform;

        // Get the material name of the collided object
        string materialName = other.transform.GetComponent<MeshRenderer>().material.name;

        // Log the material name for debugging
        Debug.Log("Material Name: " + materialName);  // Log the material name

        // Check if the material is Safe, UnSafe, or LastRing and handle accordingly
        if (materialName == "Safe (Instance)")
        {
            Debug.Log("You are Safe");
        }
        else if (materialName == "UnSafe (Instance)")
        {
            Debug.Log("You are on UnSafe"); // Log this message when the material is "UnSafe (Instance)"
            GameManager.gameOver = true;
            audio.Play("GameOver"); // Play Game Over sound
        }
        else if (materialName == "LastRing (Instance)" && !GameManager.levelWin)
        {
            GameManager.levelWin = true;
            audio.Play("levelWin"); // Play level win sound
        }
    }
}
