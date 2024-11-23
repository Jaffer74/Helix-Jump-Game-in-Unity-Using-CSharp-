using System.IO;
using UnityEngine;

public class SaveGameDataToCSV : MonoBehaviour
{
    private string filePath;

    private void Start()
    {
        // Define the path for the CSV file
        filePath = Application.dataPath + "/GameData.csv";

        // Create the file and add headers if it doesn't exist
        if (!File.Exists(filePath))
        {
            string header = "TimeTaken,Score,Level,Difficulty,Outcome";
            File.WriteAllText(filePath, header + "\n");
            Debug.Log("CSV file created with headers: " + filePath);
        }
    }

    public void SaveDataToCSV(float timeTaken, int score, int level, string difficulty, string outcome)
    {
        // Prepare the data line
        string dataLine = $"{timeTaken},{score},{level},{difficulty},{outcome}";

        // Append the data to the CSV file
        File.AppendAllText(filePath, dataLine + "\n");

        // Debug message to confirm saving
        Debug.Log($"Data saved to CSV: {dataLine}");
    }
}
