using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.ComponentModel;
//using System.Diagnostics;

public class DataManager : MonoBehaviour
{
    public TMP_InputField newExerciseName;
    public TMP_InputField changedExerciseName;
    public TMP_InputField filterExercise;
    public Toggle isLoad;
    public Toggle isReps;
    public Toggle isTime;
    public GameObject invalidName;
    public GameObject validNewName;
    public GameObject invalidChangedName;
    public TMP_Text invalidNameMessage;
    public TMP_Text invalidChangedNameMessage;
    public GameObject scrollbarExerciseContent;
    public GameObject prefabListElement;
    public Font fontInList;
    public static GameObject subMenuObject;
    public static GameObject subMenuName;
    static string currentExerciseName;
    public GameObject subMenuRemove;
    public GameObject subMenuChangeName;
    public GameObject chooseListContent;
    public GameObject chooseListPrefab;
    public GameObject chooseListStatsPrefab;
    public AssignObjects objects;
    
    void Start()
    {
        subMenuObject = objects.subMenu;
        subMenuName = objects.subMenuName;
    } 



    public void SaveNewExercise()
    {
        string nameCheck = ExerciseManager.isValidNewName(newExerciseName.text);
        if (nameCheck == "Valid name" )
        {
            ExerciseManager.AddExercise(newExerciseName.text, isLoad.isOn, isReps.isOn, isTime.isOn);
            displayExerciseList(ExerciseManager.exerciseData.exercises, prefabListElement, scrollbarExerciseContent);
            AddedNewExerciseMessage();
        }
        else
        {
            invalidNameMessage.text = nameCheck;
            invalidName.SetActive(true);
        }
    }

    public void invalidNameAccepted()
    {
        invalidName.SetActive(false);
        invalidChangedName.SetActive(false);
    }

    public void displayExerciseList(List<Exercise> exercises, GameObject prefab, GameObject parent)
    {
        //populating the list and sorting 
        List<string> names = new List<string>();
        foreach (Exercise exercise in exercises)
        {
            names.Add(exercise.name);
        }
        names.Sort();

        //clear existing text elements
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        int deltaY = 100;
        int contentHeight = 0;
        //creating Text Element for each names and display in vertical list
        foreach (string name in names)
        {
            contentHeight += deltaY;
            GameObject newPrefabInstance = Instantiate(prefab, parent.transform);
            newPrefabInstance.name = name;
            RectTransform rectTransform = newPrefabInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -contentHeight);
            Text textcomponent = newPrefabInstance.GetComponent<Text>();
            textcomponent.text = name;
        }
        RectTransform rectTransform2 = parent.GetComponent<RectTransform>();
        rectTransform2.sizeDelta = new Vector2(rectTransform2.sizeDelta.x, contentHeight);
        rectTransform2.anchoredPosition = new Vector2(0, 0);
    }

    public void displayExerciseListAll()
    {
        displayExerciseList(ExerciseManager.exerciseData.exercises, prefabListElement, scrollbarExerciseContent);
    }

    public void displayExerciseListFiltered(string filter)
    {
        List<Exercise> displayed = ExerciseManager.exerciseData.exercises.FindAll(exercise => exercise.name.ToLower().StartsWith(filter));
        displayExerciseList(displayed, prefabListElement, scrollbarExerciseContent);
    }

    public void displayChooseExerciseAll()
    {
        displayExerciseList(ExerciseManager.exerciseData.exercises, chooseListPrefab, chooseListContent);
    }

    public void displayChooseExerciseForStats()
    {
        displayExerciseList(ExerciseManager.exerciseData.exercises, chooseListStatsPrefab, chooseListContent);
    }

    public void displayChooseExerciseFiltered(string filter)
    {
        List<Exercise> displayed = ExerciseManager.exerciseData.exercises.FindAll(exercise => exercise.name.ToLower().StartsWith(filter));
        displayExerciseList(displayed, chooseListPrefab, chooseListContent);
    }

    public void removeExercise()
    {
        SubMenuRemoveEnd();
        TrainingModel.removeExerciseFromSets(currentExerciseName);
        ExerciseManager.RemoveExercise(currentExerciseName);
        displayExerciseList(ExerciseManager.exerciseData.exercises, prefabListElement, scrollbarExerciseContent);
    }

    public void changeNameExercise()
    {
        string nameCheck = ExerciseManager.isValidNewName(changedExerciseName.text);
        if (nameCheck == "Valid name")
        {
            TrainingModel.changeExerciseNameInSets(currentExerciseName, changedExerciseName.text);
            ExerciseManager.changeName(currentExerciseName, changedExerciseName.text);
            changedExerciseName.text = "";
            displayExerciseList(ExerciseManager.exerciseData.exercises, prefabListElement, scrollbarExerciseContent);
            SubMenuChangeNameEnd();
            SubMenuEnd();
        }
        else
        {
            invalidChangedNameMessage.text = nameCheck;
            invalidChangedName.SetActive(true);
        }
    }

    public static void SubMenuStart(string title)
    {
        subMenuObject.SetActive(true);
        TextMeshProUGUI textComponent = subMenuName.GetComponent<TextMeshProUGUI>();
        currentExerciseName = title;
        textComponent.text = title;
    }

    public static void SubMenuEnd()
    {
        subMenuObject.SetActive(false);
    }

    public void SubMenuRemoveStart()
    {
        subMenuRemove.SetActive(true);
    }

    public void SubMenuRemoveEnd()
    {
        subMenuRemove.SetActive(false);
    }

    public void SubMenuChangeNameStart()
    {
        subMenuChangeName.SetActive(true);
        changedExerciseName.text = currentExerciseName;
    }

    public void SubMenuChangeNameEnd()
    {
        subMenuChangeName.SetActive(false);
    }

    public void AddedNewExerciseMessage()
    {
        validNewName.SetActive(true);
    }

    public void AddedNewExerciseMessageOff()
    {
        validNewName.SetActive(false);
    }

    public void clearInputField()
    {
        if (validNewName.activeSelf)
        {
            newExerciseName.text = "";
        }
    }

    public void statsOfExercise()
    {
        TextMeshProUGUI textComponent = subMenuName.GetComponent<TextMeshProUGUI>();
        Statistics.currentExercise = textComponent.text;
        Statistics.mode = 1;
        Statistics.periods = 0;
        Statistics.displayExerciseStatsFlag = true;
    }

}