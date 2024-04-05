using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Globalization;
using TMPro;

public class Statistics : MonoBehaviour
{
    public GameObject WhenLastContent;
    public GameObject WhenLastPrefab;
    public GameObject GeneralStatsContent;
    public GameObject GeneralStatsPrefab;
    public TMP_Dropdown GeneralStatsDropdown;
    public static string currentExercise;
    public static int periods = 0;
    public static int mode = 0;
    public static bool displayExerciseStatsFlag = false;
    public GameObject exerciseName;
    public GameObject statsLabelReps;
    public GameObject statsLabelLoad;
    public GameObject statsLabelTime;

    void Start()
    {
        ExerciseManager.LoadExerciseData();
        TrainingModel.loadTrainingData();
        TrainingModel.loadSetData();

        displayWhenLastTime();
    }

    void Update()
    {
        if (displayExerciseStatsFlag)
        {
            displayExerciseStatsFlag = false;
            //wykonaj funkcje
            displayExerciseStats(currentExercise, periods, mode);
        }
    }

    public Dictionary<string, int> whenLastTime()
    {
        Dictionary<string, int> dict = new Dictionary<string, int>();

        foreach (Exercise exercise in ExerciseManager.exerciseData.exercises)
        {
            //find last training with this exercise
            int lastTrainingIndex = TrainingModel.setInfoData.setsInfo
            .Where(setInfo => setInfo.exerciseName == exercise.name)
            .Select(setInfo => setInfo.trainingIndex)
            .DefaultIfEmpty(-1) // return -1 if no matching exercise is found
            .Max();

            if (lastTrainingIndex == -1)
            {
                continue;
            }

            // Find the date of the last occurrence 
            var lastDate = TrainingModel.trainingData.trainings
                .Where(training => training.trainingIndex == lastTrainingIndex)
                .Select(training => new DateTime(training.yearNumber, training.monthNumber, training.dayNumber))
                .SingleOrDefault();

            //count how many days before
            int daysSinceLastOccurrence = (DateTime.Now - lastDate).Days;

            dict[exercise.name] = daysSinceLastOccurrence;
        }

        // Sorting the dictionary by value and returning
        return dict.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void displayWhenLastTime()
    {
        Dictionary<string, int> dict = whenLastTime();

        foreach (Transform child in WhenLastContent.transform)
        {
            Destroy(child.gameObject);
        }

        int y = 0;
        foreach (var kvp in dict)
        {
            //wstaw prefab
            GameObject newPrefabInstance = Instantiate(WhenLastPrefab, WhenLastContent.transform);
            //ustaw pozycje prefabu i zwiêksz content area
            RectTransform rectTransform = newPrefabInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -y);
            y += 100;
            rectTransform = WhenLastContent.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, y);
            //dostaosuj napisy prafubu
            Text textComponent = newPrefabInstance.GetComponent<Text>();
            textComponent.text = kvp.Key;
            GameObject gameObject = newPrefabInstance.transform.Find("When").gameObject;
            textComponent = gameObject.GetComponent<Text>();
            textComponent.text = kvp.Value.ToString() + " days";
        }
    }

    public void displayGeneralStats(int dropDown)
    {
        int rowLimit = 20;
        string periods;
        string thisPeriod;
        List<Training> trainings = new();
        List<string> months = new List<string> { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "nov", "oct", "dec" };
        int yDelta = 80;
        int yPosition = yDelta;
        RectTransform rectTransform;

        if (dropDown == 0) periods = "days";
        else if (dropDown == 1) periods = "weeks";
        else if (dropDown == 2) periods = "months";
        else if (dropDown == 3) periods = "years";
        else return;

        System.DateTime dateTime = System.DateTime.Now;

        foreach (Transform child in GeneralStatsContent.transform)
        {
            Destroy(child.gameObject);
        }

        int firstYear = TrainingModel.trainingData.trainings
        .OrderBy(training => training.trainingIndex)
        .Select(training => training.yearNumber)
        .FirstOrDefault();

        int firstMonth = TrainingModel.trainingData.trainings
        .OrderBy(training => training.trainingIndex)
        .Select(training => training.monthNumber)
        .FirstOrDefault();

        int firstDay = TrainingModel.trainingData.trainings
        .OrderBy(training => training.trainingIndex)
        .Select(training => training.dayNumber)
        .FirstOrDefault();

        for (int i = 0; i < rowLimit; i++)
        {
            yPosition -= yDelta;
            int weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);

            if (periods == "days")
            {
                trainings = TrainingModel.trainingData.trainings.Where(training => training.yearNumber == dateTime.Year && training.monthNumber == dateTime.Month && training.dayNumber == dateTime.Day).ToList();
                thisPeriod = dateTime.Day.ToString() + " " + months[dateTime.Month - 1];
                dateTime = dateTime.AddDays(-1);
            }
            else if (periods == "weeks")
            {
                trainings = TrainingModel.trainingData.trainings.Where(training => training.yearNumber == dateTime.Year && training.weekNumber == weekOfYear).ToList();
                thisPeriod = "week " + weekOfYear.ToString();
                dateTime = dateTime.AddDays(-7);
            }
            else if (periods == "months")
            {
                trainings = TrainingModel.trainingData.trainings.Where(training => training.yearNumber == dateTime.Year && training.monthNumber == dateTime.Month).ToList();
                thisPeriod = months[dateTime.Month - 1] + " " + dateTime.Year.ToString();
                dateTime = dateTime.AddMonths(-1);
            }
            else if (periods == "years")
            {
                trainings = TrainingModel.trainingData.trainings.Where(training => training.yearNumber == dateTime.Year).ToList();
                thisPeriod = dateTime.Year.ToString();
                dateTime = dateTime.AddYears(-1);
            }
            else
            {
                Debug.Log("coœ jest nie tak");
                return;
            }

            int TrainingCount = trainings.Count();
            int totalTime = 0;
            int totalSets = 0;
            foreach (Training training in trainings)
            {
                totalTime += training.trainingTime;
                totalSets += training.setCount;
            }

            //wrzuæ prefab
            GameObject newPrefabInstance = Instantiate(GeneralStatsPrefab, GeneralStatsContent.transform);
            //dostosuj napisy
            GameObject gameObject = newPrefabInstance.transform.Find("Period").gameObject;
            TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = thisPeriod;

            gameObject = newPrefabInstance.transform.Find("Trainings").gameObject;
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = TrainingCount.ToString();

            gameObject = newPrefabInstance.transform.Find("Sets").gameObject;
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = totalSets.ToString();

            gameObject = newPrefabInstance.transform.Find("Time").gameObject;
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = totalTime.ToString() + " min";

            //dostosuj po³o¿enie
            rectTransform = newPrefabInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosition);


            if (firstYear > dateTime.Year) break;
            else if (firstYear == dateTime.Year && firstMonth > dateTime.Month) break;
            else if (firstYear == dateTime.Year && firstMonth == dateTime.Month && firstDay > dateTime.Day) break;

        }

        rectTransform = GeneralStatsContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.anchoredPosition.x, - yPosition + yDelta);
    }

    public void displayGeneralStatsDef()
    {
        GeneralStatsDropdown.value = 0;
    }

    public void displayExerciseStats(string exerciseName, int periods, int mode)
    {
        TextMeshProUGUI textComponent = this.exerciseName.GetComponent<TextMeshProUGUI>();
        textComponent.text = exerciseName;

        Exercise exercise = ExerciseManager.exerciseData.exercises
            .Where(ex => ex.name == exerciseName)
            .SingleOrDefault();

        if (exercise.onReps) statsLabelReps.SetActive(true);
        else statsLabelReps.SetActive(false);
        if (exercise.onLoad) statsLabelLoad.SetActive(true);
        else statsLabelLoad.SetActive(false);
        if (exercise.onTime) statsLabelTime.SetActive(true);
        else statsLabelTime.SetActive(false);



    }

}
