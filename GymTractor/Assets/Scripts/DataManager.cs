using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.ComponentModel;

public class DataManager : MonoBehaviour
{
    public TMP_InputField newExerciseName;
    public Toggle isLoad;
    public Toggle isReps;
    public Toggle isTime;
    public GameObject invalidName;
    public TMP_Text invalidNameMessage;
    public static bool invalidNameFlag = false;
    public GameObject scrollbarExerciseContent;
    public Font fontInList;

    void Start()
    {
        ExerciseManager.LoadExerciseData();
    }

    public void SaveNewExercise()
    {
        string nameCheck = ExerciseManager.isValidNewName(newExerciseName.text);
        if (nameCheck == "Valid name" && !invalidNameFlag)
        {
            ExerciseManager.AddExercise(newExerciseName.text, isLoad.isOn, isReps.isOn, isTime.isOn);
            newExerciseName.text = "";
            displayExerciseList();
        }
        else
        {
            invalidNameFlag = true;
            invalidNameMessage.text = nameCheck;
            invalidName.SetActive(true);
        }
    }

    public void invalidNameAccepted()
    {
        invalidNameFlag = false;
        invalidName.SetActive(false);
    }

    public void displayExerciseList()
    {
        GameObject parent = scrollbarExerciseContent;
        //populating the list and sorting 
        List<string> names = new List<string>();
        foreach (Exercise exercise in ExerciseManager.exerciseData.exercises)
        {
            names.Add(exercise.name);
        }
        names.Sort();

        //clear existing text elements
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        int PositionY = -120;
        int deltaY = 100;
        //creating Text Element for each names and display in vertical list
        foreach (string name in names)
        {
            GameObject textGameObject = new GameObject(name);
            textGameObject.transform.SetParent(parent.transform, false);
            textGameObject.transform.localPosition = new Vector3(20, PositionY, 0);
            Destroy(textGameObject.GetComponent<Transform>());

            RectTransform rectTransform = textGameObject.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0f, 0f);
            rectTransform.sizeDelta = new Vector2(600, deltaY - 10);

            Text textComponent = textGameObject.AddComponent<Text>();
            textComponent.text = name;
            textComponent.font = fontInList;
            textComponent.fontSize = 50;

            PositionY -= deltaY;
        }

        RectTransform rectTransform2 = parent.GetComponent<RectTransform>();
        rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, -PositionY - deltaY / 2);
    }

}
