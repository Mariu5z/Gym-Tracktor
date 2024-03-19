using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;



public class TrainingData
{
    public List<Training> trainings;
}

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



public static class GymTractorData
{
    //static public string trainingFilePath = Application.persistentDataPath + "/TrainingsData.json";//file for storing data
    //static public string setInfoFilePath = Application.persistentDataPath + "/SetsData.json";//file for storing data

}
