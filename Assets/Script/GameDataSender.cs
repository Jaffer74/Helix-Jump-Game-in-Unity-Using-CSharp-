using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class GameDataSender : MonoBehaviour
{
    private string apiUrl = "http://127.0.0.1:5000/predict";  // Flask API URL

    // Method to send game data to Flask API
    public void SendDataToAPI(float timeTaken, int score, int level, string difficulty, string status)
    {
        StartCoroutine(PostGameData(timeTaken, score, level, difficulty, status));
    }

    // Coroutine to send data to the API using UnityWebRequest
    private IEnumerator PostGameData(float timeTaken, int score, int level, string difficulty, string status)
    {
        // Create the JSON object to send
        string jsonData = JsonUtility.ToJson(new GameData(timeTaken, score, level, difficulty, status));

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Wait for the API response
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Log the response from Flask (i.e., "won" or "lost")
            Debug.Log("Server Response: " + request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Failed to send data: " + request.error);
        }
    }
}

// Serializable class for storing the game data to send
[System.Serializable]
public class GameData
{
    public float timeTaken;
    public int score;
    public int level;
    public string difficulty;
    public string status; // Win or loss

    public GameData(float timeTaken, int score, int level, string difficulty, string status)
    {
        this.timeTaken = timeTaken;
        this.score = score;
        this.level = level;
        this.difficulty = difficulty;
        this.status = status; // Win or loss
    }
}
