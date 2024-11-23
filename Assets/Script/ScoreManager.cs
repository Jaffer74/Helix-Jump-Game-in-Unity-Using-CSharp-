using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the score UI text
    private int score = 0; // Keeps track of the score

    // This method will be called whenever the ball passes a ring
    public void AddScore(int value)
    {
        score += value; // Increase the score
        UpdateScoreUI(); // Update the score displayed in the UI
    }

    // Update the UI text with the current score
    private void UpdateScoreUI()
    {
        scoreText.text = score.ToString(); // Convert the score to a string
    }
}
