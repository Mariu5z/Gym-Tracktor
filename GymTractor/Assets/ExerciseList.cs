using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExerciseList : MonoBehaviour
{

    public void displayExerciseList()
    {
        //populating the list and sorting 
        List<string> names = new List<string>();
        foreach (Exercise exercise in ExerciseManager.exerciseData.exercises)
        {
            names.Add(exercise.name);
        }
        names.Sort();

        //clear existing text elements
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        int PositionY = 200;
        int deltaY = 100;
        //creating Text Element for each names and display in vertical list
        foreach (string name in names)
        {
            GameObject textGameObject = new GameObject(name);
            textGameObject.transform.SetParent(gameObject.transform, false);
            textGameObject.transform.localPosition = new Vector3(20, PositionY, 0);

            RectTransform rectTransform = textGameObject.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.pivot = new Vector2(0f, 0f);
            rectTransform.sizeDelta = new Vector2(600, deltaY - 10);

            Text textComponent = textGameObject.AddComponent<Text>();
            textComponent.text = name;
            textComponent.fontSize = 40;

            PositionY += deltaY;
        }

    }
}
