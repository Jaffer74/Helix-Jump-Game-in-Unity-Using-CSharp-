using System.IO;
using UnityEngine;

public class DataPreparation : MonoBehaviour
{
    // Path to store the CSV file in persistent data path
    private string dataFilePath;

    // Start is called before the first frame update
    void Awake()
    {
        // Ensure the dataFilePath points to the correct location
        dataFilePath = Path.Combine(Application.persistentDataPath, "game_data.csv");
        
        // Log the path for debugging purposes
        Debug.Log("Data file path: " + dataFilePath);
        
        // Call the method to load and process data
        LoadAndProcessData();
    }

    // Method to load and process data from the CSV file
    void LoadAndProcessData()
    {
        // Check if the file exists
        if (File.Exists(dataFilePath))
        {
            // If file exists, load its content
            string data = File.ReadAllText(dataFilePath);
            ProcessData(data);
        }
        else
        {
            // If file does not exist, create a default file
            Debug.LogWarning("Data file not found at: " + dataFilePath);
            CreateDefaultDataFile();
        }
    }

    // Method to create a default CSV file with basic data
    void CreateDefaultDataFile()
    {
        // Create a string representing the default data for CSV (modify this data as needed)
        string defaultData = "Ring1, 5\nRing2, 10\nRing3, 7\nRing4, 8";  // Modify with actual data format if needed
        
        // Ensure the directory exists before writing the file
        string directoryPath = Path.GetDirectoryName(dataFilePath);
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
            Debug.Log("Directory created: " + directoryPath);
        }

        // Write the default data to the CSV file
        try
        {
            File.WriteAllText(dataFilePath, defaultData);
            Debug.Log("Default data file created at: " + dataFilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to create file: " + e.Message);
        }

        // Now load and process the newly created file
        ProcessData(defaultData);
    }

    // Method to process the data (example method, modify based on your actual logic)
    void ProcessData(string data)
    {
        // Example: just log the data for now (you can implement your own processing logic here)
        Debug.Log("Processing data: " + data);
        
        // You can now split the data by lines or commas to use it as per your game logic
        string[] lines = data.Split('\n');
        foreach (var line in lines)
        {
            string[] values = line.Split(',');
            Debug.Log("Ring Name: " + values[0] + ", Value: " + values[1]);
        }
    }
}
