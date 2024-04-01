using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class TrainingData
{
    public List<Training> trainings;
}

[System.Serializable]
public class SetInfoData
{
    public List<SetInfo> setsInfo;
}

[System.Serializable]
public class Training
{
    public int trainingIndex;//primary key
    public int trainingTime;
    public int dayNumber;
    public int weekNumber;
    public int monthNumber;
    public int yearNumber;

    public Training(int index, int time, int day, int week, int month, int year)
    {
        trainingIndex = index;
        trainingTime = time;
        dayNumber = day;
        weekNumber = week;
        monthNumber = month;
        yearNumber = year;
    }

}

[System.Serializable]
public class SetInfo
{
    public int setIndex;//primary key
    public int trainingIndex;
    public string exerciseName;
    public float load;
    public int reps;
    public int time;
}


public static class TrainingModel
{
    public static string TrainingFilePath = Application.persistentDataPath + "/TrainingsData.json";//file for storing data
    public static string SetInfoFilePath = Application.persistentDataPath + "/SetsData.json";//file for storing data
    public static TrainingData trainingData;
    public static SetInfoData setInfoData;

    public static void loadTrainingData ()
    {
        if (File.Exists(TrainingFilePath))
        {
            //Debug.Log("Training File exists");
            //Debug.Log(TrainingFilePath);
            string json = File.ReadAllText(TrainingFilePath);
            trainingData = JsonUtility.FromJson<TrainingData>(json);
        }
        else
        {
            //Debug.Log("Training File doesnt exists");
            trainingData = new TrainingData();
            trainingData.trainings = new();
            saveData(TrainingFilePath, trainingData);
        }
    }

    public static void loadSetData()
    {
        if (File.Exists(SetInfoFilePath))
        {
            //Debug.Log("Set File exists");
            string json = File.ReadAllText(SetInfoFilePath);
            setInfoData = JsonUtility.FromJson<SetInfoData>(json);
        }
        else
        {
            //Debug.Log("Set File doesnt exists");
            //Debug.Log(SetInfoFilePath);
            setInfoData = new SetInfoData();
            setInfoData.setsInfo = new();
            saveData(SetInfoFilePath, setInfoData);
        }
    }

    //Save training data
    public static void saveData<T>(string filepath, T data)
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(filepath, json);
    }

    //Add new training
    public static void addNewTrainingData(int day, int week, int month, int year, int minutes)
    {
        loadTrainingData();
        Training training = new Training(trainingData.trainings.Count + 1, minutes, day, week, month, year);
        trainingData.trainings.Add(training);
        saveData(TrainingFilePath, trainingData);
    }

    public static void addNewSetInfoData(string exercise, float load, int reps, int time)
    {
        loadTrainingData();
        loadSetData();

        SetInfo setInfo = new SetInfo
        {
            setIndex = setInfoData.setsInfo.Count + 1,//primary key
            trainingIndex = trainingData.trainings.Count,
            exerciseName = exercise,
            load = load,
            reps = reps,
            time = time
        };

        setInfoData.setsInfo.Add(setInfo);
        saveData(SetInfoFilePath, setInfoData);
    }

}
