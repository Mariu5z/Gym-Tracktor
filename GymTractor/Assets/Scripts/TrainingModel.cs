using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Globalization;

[System.Serializable]
public class TrainingData
{
    public int startDay;
    public int startMonth;
    public int startYear;
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
    public int setCount;

    public Training(int index, int time, int day, int week, int month, int year, int sets)
    {
        trainingIndex = index;
        trainingTime = time;
        dayNumber = day;
        weekNumber = week;
        monthNumber = month;
        yearNumber = year;
        setCount = sets;
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
    public static string startDateFilePath = Application.persistentDataPath + "/startDate.json";//file for storing data
    public static TrainingData trainingData;
    public static SetInfoData setInfoData;

    public static void loadTrainingData()
    {
        if (File.Exists(TrainingFilePath))
        {
            //Debug.Log(TrainingFilePath);
            string json = File.ReadAllText(TrainingFilePath);
            trainingData = JsonUtility.FromJson<TrainingData>(json);
        }
        else
        {
            trainingData = new TrainingData();
            trainingData.trainings = new();
            trainingData.startDay = System.DateTime.Now.Day;
            trainingData.startMonth = System.DateTime.Now.Month;
            trainingData.startYear = System.DateTime.Now.Year;
            saveData(TrainingFilePath, trainingData);
        }
    }

    public static void loadSetData()
    {
        if (File.Exists(SetInfoFilePath))
        {
            string json = File.ReadAllText(SetInfoFilePath);
            setInfoData = JsonUtility.FromJson<SetInfoData>(json);
        }
        else
        {
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
        //Debug.Log("Data saved");
    }

    //Add new training
    public static void addNewTrainingData(int day, int week, int month, int year, int minutes, int sets)
    {
        //loadTrainingData();
        int lastTrainingIndex = trainingData.trainings
        .Select(training => training.trainingIndex)
        .DefaultIfEmpty(0)
        .Max();

        Training training = new Training(lastTrainingIndex + 1, minutes, day, week, month, year, sets);
        trainingData.trainings.Add(training);
        saveData(TrainingFilePath, trainingData);
    }

    public static void addNewSetInfoData(string exercise, float load, int reps, int time)
    {
        //loadTrainingData();
        //loadSetData();
        int lastSetIndex = setInfoData.setsInfo
        .Select(set => set.setIndex)
        .DefaultIfEmpty(0)
        .Max();

        int lastTrainingIndex = trainingData.trainings
        .Select(training => training.trainingIndex)
        .DefaultIfEmpty(0)
        .Max();

        SetInfo setInfo = new SetInfo
        {
            setIndex = lastSetIndex + 1,//primary key
            trainingIndex = lastTrainingIndex,
            exerciseName = exercise,
            load = load,
            reps = reps,
            time = time
        };

        setInfoData.setsInfo.Add(setInfo);
        saveData(SetInfoFilePath, setInfoData);
    }

    public static void removeExerciseFromSets(string exercise)
    {
        setInfoData.setsInfo.RemoveAll(set => set.exerciseName == exercise);
        saveData(SetInfoFilePath, setInfoData);
    }

    public static void changeExerciseNameInSets(string oldName, string newName)
    {
        foreach (SetInfo setInfo in TrainingModel.setInfoData.setsInfo)
        {
            if (setInfo.exerciseName == oldName)
            {
                setInfo.exerciseName = newName;
            }
        }
        saveData(SetInfoFilePath, setInfoData);
    }

    public static bool isBeforeStartDate(int Day, int Month, int Year)
    {
        if (trainingData.startYear > Year) return true;
        else if (trainingData.startYear == Year && trainingData.startMonth > Month) return true;
        else if (trainingData.startYear == Year && trainingData.startMonth == Month && trainingData.startDay > Day) return true;
        else return false;
    }

}
