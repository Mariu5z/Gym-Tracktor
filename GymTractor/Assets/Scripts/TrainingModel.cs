using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


public class TrainingModel : MonoBehaviour
{
    static public string filePath = Application.persistentDataPath + "/TrainingsData.json";//file for storing data
    TrainingData trainingData = new TrainingData();

    //Load trainin data

    //Save training data

    //Add new training

    //Add new SetInfo

    //change exercise name in set info
    
}
