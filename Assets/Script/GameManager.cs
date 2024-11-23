using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class GameManager : MonoBehaviour
{
    // Define CurrentLevelIndex as static to keep track of the level across scenes
    public static int CurrentLevelIndex = 1;

    // Other variables
    public static bool gameOver;
    public static bool levelWin;
    public static bool mute = false;

    public Button sendDataButton;  // Declare the sendDataButton variable

    // UI Elements
    public GameObject gameOverPanel;
    public GameObject levelWinPanel;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public Slider progressBar;
    public TextMeshProUGUI scoreText; // Added to display the score on UI

    // Variables for progress bar and level tracking
    public static int noOfPassingRings;

    // Tracking Metrics
    public static float levelCompletionTime = 0f;   // Time to complete the level
    public static int levelFailures = 0;             // Number of failures
    public static int score = 0;                     // Player's score
    public static string playerActions = "";         // Store player actions (simple string for now)
    public static string levelDifficulty = "Easy";   // Default difficulty level

    public GameDataSender gameDataSender; // Reference to GameDataSender script

    void Awake()
    {
        // Initialize the level from PlayerPrefs or default to 1
        CurrentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 1);
    }

    private void Start()
    {
        // Set initial game state
        Time.timeScale = 1f;
        noOfPassingRings = 0;
        gameOver = false;
        levelWin = false;

        // Initialize tracking variables
        levelCompletionTime = 0f;
        levelFailures = 0;
        score = 0;
        playerActions = "";  // Reset actions
        levelDifficulty = GetLevelDifficulty(CurrentLevelIndex); // Get difficulty based on level

        // Update UI with current level and next level
        currentLevelText.text = CurrentLevelIndex.ToString();
        nextLevelText.text = (CurrentLevelIndex + 1).ToString();

        // Initialize progress bar max value and starting value
        progressBar.maxValue = 100; // Assuming 100 is the max value
        progressBar.value = 0;

        // Load saved data if available
        LoadData();
        sendDataButton.onClick.AddListener(() => EndGame(levelWin ? "win" : "loss")); // Use lambda to pass status
    }

    private void Update()
    {
        // Update the level difficulty every time the level changes
        levelDifficulty = GetLevelDifficulty(CurrentLevelIndex);  // Ensure difficulty is updated

        // Log current level and difficulty for debugging purposes
        Debug.Log($"Current Level: {CurrentLevelIndex}, Difficulty: {levelDifficulty}");

        // Track the time taken to complete the level
        if (!gameOver && !levelWin)
        {
            levelCompletionTime += Time.deltaTime; // Increment time
        }

        // If game is over, stop the game and show game over screen
        if (gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
            levelFailures++; // Increment failure count when the game is over
            SaveData(); // Save data when the game is over
            if (Input.GetMouseButtonDown(0))
            {
                // Reload the scene when the player clicks
                SceneManager.LoadScene(0);
            }
        }

        // Make sure currentLevelText and nextLevelText are assigned
            if (currentLevelText != null)
            {
                currentLevelText.text = GameManager.CurrentLevelIndex.ToString();
            }

    // Ensure next level text is updated
            if (nextLevelText != null)
            {
                nextLevelText.text = (GameManager.CurrentLevelIndex + 1).ToString();
            }

    // Update progress bar dynamically
            if (progressBar != null && FindObjectOfType<HelixManager>() != null)
            {
                int progress = noOfPassingRings * 100 / FindObjectOfType<HelixManager>().noOfRings;
                progressBar.value = progress;
            }       

        else
        {
            Debug.LogWarning("ProgressBar or HelixManager is not assigned correctly.");
        }

        // If level is won, show level win screen
        if (levelWin)
        {
            levelWinPanel.SetActive(true);
            SaveData(); // Save data when the level is won
            if (Input.GetMouseButtonDown(0))
            {
                // Save current level and go to the next level
                PlayerPrefs.SetInt("CurrentLevelIndex", CurrentLevelIndex + 1);
                SceneManager.LoadScene(0);
            }
        }
    }

    // Method to toggle mute state
    public static void ToggleMute()
    {
        mute = !mute;
    }

    // Method to calculate and save the data (time, failures, score, difficulty, actions)
    public void SaveData()
    {
        string path = Application.persistentDataPath + "/player_data.csv";
        string data = $"{levelCompletionTime},{levelFailures},{score},{CurrentLevelIndex},{levelDifficulty},{playerActions}\n"; // Include actions and difficulty

        // Save data to the file
        try
        {
            File.AppendAllText(path, data);
            Debug.Log("Data saved: " + data);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving data: " + e.Message);
        }
    }

    // Example method for calculating score based on time and failures (you can customize it)
    public void CalculateScore()
    {
        // A simple scoring system: More time and more failures reduce the score
        score = Mathf.Max(0, 1000 - Mathf.RoundToInt(levelCompletionTime * 10) - (levelFailures * 100));
    }

    // Method to add score
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;  // Add points
        Debug.Log("Score added: " + scoreToAdd + ", Total Score: " + score); // Debug log to check score
        UpdateScoreUI();      // Update the score display
    }

    // Method to update the score UI
    private void UpdateScoreUI()
    {
        // Update the score on the UI (you need to make sure scoreText is assigned)
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    // Method to determine the difficulty based on the level
    private string GetLevelDifficulty(int level)
    {
        // Simple difficulty logic based on level (this can be more complex)
        if (level <= 5)
            return "Easy";
        else if (level <= 10)
            return "Medium";
        else
            return "Hard";
    }

    // Method to load data (you can load saved progress or scores here)
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/player_data.csv";

        // Check if the file exists
        if (File.Exists(path))
        {
            // Read all lines from the saved file
            string[] lines = File.ReadAllLines(path);

            foreach (string line in lines)
            {
                string[] data = line.Split(',');

                // Example: Parse data (levelCompletionTime, levelFailures, etc.)
                if (data.Length >= 6)
                {
                    levelCompletionTime = float.Parse(data[0]);
                    levelFailures = int.Parse(data[1]);
                    score = int.Parse(data[2]);
                    CurrentLevelIndex = int.Parse(data[3]);
                    levelDifficulty = data[4];

                    Debug.Log($"Loaded Data: {levelCompletionTime}, {levelFailures}, {score}, {CurrentLevelIndex}, {levelDifficulty}");
                }
            }
        }
        else
        {
            Debug.LogWarning("No saved data found.");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the ball fell on an unsafe area
        if (collision.gameObject.CompareTag("Unsafe"))
        {
            Debug.Log("Game Over: Loss!");
            gameOver = true; // Set the game state to over
            EndGame("loss"); // Send "loss" status when the game ends
        }

        // Check if the ball hits the passing rings
        if (collision.gameObject.CompareTag("PassingRing"))
        {
            AddScore(1);
            noOfPassingRings++; // Increment number of passing rings
            Debug.Log($"Rings Passed: {noOfPassingRings}");
        }
    }

    // End the game, send data to the API
    public void EndGame(string status)
    {
        CalculateScore(); // Calculate the score based on current game state

        // Send data (time, score, level, difficulty, status)
        gameDataSender.SendDataToAPI(levelCompletionTime, score, CurrentLevelIndex, levelDifficulty, status);
    }
}
