using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PredictionManager : MonoBehaviour
{
    public InputField timeTakenField;
    public InputField failuresField;
    public InputField scoreField;
    public InputField difficultyLevelField;
    public Text resultText;

    private string apiUrl = "http://localhost:5000/predict";  // Flask API URL

    // Call this method to send data and get predictions
    public void GetPrediction()
    {
        // Prepare input data
        float timeTaken = float.Parse(timeTakenField.text);
        int failures = int.Parse(failuresField.text);
        int score = int.Parse(scoreField.text);
        int difficultyLevel = int.Parse(difficultyLevelField.text);

        // Create a JSON object
        string jsonData = "{\"features\": [" + timeTaken + "," + failures + "," + score + "," + difficultyLevel + "]}";

        // Send the data to the Flask API
        StartCoroutine(SendPredictionRequest(jsonData));
    }

    // Coroutine to send the POST request to the Flask API
    private IEnumerator SendPredictionRequest(string jsonData)
    {
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Wait for the request to complete
        yield return request.SendWebRequest();

        // Check for request errors
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            resultText.text = "Error: " + request.error;
        }
        else
        {
            // Display the prediction result
            string response = request.downloadHandler.text;
            resultText.text = "Prediction: " + response;
        }
    }
}
