using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

[System.Serializable]
public class ExerciseData
{
    public List<Exercise> exercises;
}

[System.Serializable]
public class Exercise
{
    public string name;//primary key
    public bool onLoad;
    public bool onReps;
    public bool onTime;
}


public static class ExerciseManager
{
    static public string filePath = Application.persistentDataPath + "/ExercisesData.json";//file for storing data
    static public ExerciseData exerciseData;

    static public void LoadExerciseData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            exerciseData = JsonUtility.FromJson<ExerciseData>(json);
            //Debug.Log("File exists");
        }
        else
        {
            exerciseData = new ExerciseData();
            //Debug.Log("File doesnt exists");
            SaveExerciseData();
        }
    }

    static public void SaveExerciseData()
    {
        string json = JsonUtility.ToJson(exerciseData);
        File.WriteAllText(filePath, json);
    }

    static public void AddExercise(string name, bool onLoad, bool onReps, bool onTime)
    {
        Exercise newExercise = new Exercise
        {
            name = name,
            onLoad = onLoad,
            onReps = onReps,
            onTime = onTime
        };
        exerciseData.exercises.Add(newExercise);
        SaveExerciseData();
    }

    static public void RemoveExercise(string name)
    {
        exerciseData.exercises.RemoveAll(exercise => exercise.name == name);
        SaveExerciseData();
    }

    static public string isValidNewName(string name)
    {
        //check if data initialized
        if (exerciseData == null || exerciseData.exercises == null)
        {
            return "No database available";
        }

        //check if name has invalid number of characters
        if (name.Length < 1 || name.Length > 28)
        {
            return "Wrong name length";
        }

        //check if this name already is in database
        if (exerciseData.exercises.Any(exercise => exercise.name == name))
        {
            return "This name already exists";
        }

        //valid new name
        return "Valid name";
    }

    public static void changeName(string nameOld, string nameNew)
    {
        foreach (Exercise exercise in ExerciseManager.exerciseData.exercises)
        {
            if (exercise.name == nameOld)
            {
                exercise.name = nameNew;
                return;
            }
        }
        SaveExerciseData();
    }
}
