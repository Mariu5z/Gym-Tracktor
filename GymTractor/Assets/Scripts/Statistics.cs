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
    public TMP_Dropdown ExerciseStatsPeriods;
    public TMP_Dropdown ExerciseStatsMode;
    public static string currentExercise;
    public static int periods = 0;
    public static int mode = 0;
    public static bool displayExerciseStatsFlag = false;
    public GameObject exerciseNameObject;
    public GameObject statsLabelReps;
    public GameObject statsLabelLoad;
    public GameObject statsLabelTime;
    public GameObject exerciseStatsPrefab;
    public GameObject exerciseStatsContent;
    List<string> months = new List<string> { "jan", "feb", "mar", "apr", "may", "jun", "jul", "aug", "sep", "nov", "oct", "dec" };

    void Start()
    {
        ExerciseManager.LoadExerciseData();
        TrainingModel.loadTrainingData();
        TrainingModel.loadSetData();

        RectTransform rectTransform = GeneralStatsContent.transform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);

        displayWhenLastTime();
    }

    void Update()
    {
        if (displayExerciseStatsFlag)
        {
            displayExerciseStatsFlag = false;
            //wykonaj funkcje
            ExerciseStatsPeriods.SetValueWithoutNotify(periods);
            ExerciseStatsMode.SetValueWithoutNotify(mode);
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
        int yDelta = 80;
        int yPosition = 0;
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

            if (TrainingModel.isBeforeStartDate(dateTime.Day, dateTime.Month, dateTime.Year)) break;
        }

        rectTransform = GeneralStatsContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.anchoredPosition.x, - yPosition);
    }

    public void displayGeneralStatsDef()
    {
        displayGeneralStats(GeneralStatsDropdown.value);
    }

    public void displayExerciseStats(string exerciseName, int periods, int mode)
    {
        TextMeshProUGUI textComponent = exerciseNameObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = exerciseName;
        GameObject gameObject;
        RectTransform rectTransform;
        int yDelta = 80;
        int yPosition = 0;
        int rowLimit = 20;
        string thisPeriod = "-";
        List<SetInfo> sets = new List<SetInfo>();
        List<int> trainings = new List<int>();

        Exercise exercise = ExerciseManager.exerciseData.exercises
            .Where(ex => ex.name == exerciseName)
            .SingleOrDefault();

        System.DateTime dateTime = System.DateTime.Now;
        int weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);

        //widocznoœæ nag³ówków
        if (exercise.onReps) statsLabelReps.SetActive(true);
        else statsLabelReps.SetActive(false);
        if (exercise.onLoad) statsLabelLoad.SetActive(true);
        else statsLabelLoad.SetActive(false);
        if (exercise.onTime) statsLabelTime.SetActive(true);
        else statsLabelTime.SetActive(false);

        //wyczyœæ ostatnio wyœwietlane
        foreach (Transform child in exerciseStatsContent.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < rowLimit; i++)
        {
            //getting sets within that period with this exercise
            if (periods == 0)
            {
                //days
                trainings = TrainingModel.trainingData.trainings
                    .Where(training => training.yearNumber == dateTime.Year && training.monthNumber == dateTime.Month && training.dayNumber == dateTime.Day)
                    .Select(training => training.trainingIndex)
                    .ToList();
                thisPeriod = dateTime.Day.ToString() + " " + months[dateTime.Month - 1];
                dateTime = dateTime.AddDays(-1);
            }
            else if (periods == 1)
            {
                //weeks
                trainings = TrainingModel.trainingData.trainings
                    .Where(training => training.yearNumber == dateTime.Year && training.weekNumber == weekOfYear)
                    .Select(training => training.trainingIndex)
                    .ToList();
                thisPeriod = "week " + weekOfYear.ToString();
                dateTime = dateTime.AddDays(-7);
                weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);
            }
            else if (periods == 2)
            {
                //months
                trainings = TrainingModel.trainingData.trainings
                    .Where(training => training.yearNumber == dateTime.Year && training.monthNumber == dateTime.Month)
                    .Select(training => training.trainingIndex)
                    .ToList();
                thisPeriod = months[dateTime.Month - 1] + " " + dateTime.Year.ToString();
                dateTime = dateTime.AddMonths(-1);
            }
            else if (periods == 3)
            {
                //years
                trainings = TrainingModel.trainingData.trainings
                    .Where(training => training.yearNumber == dateTime.Year)
                    .Select(training => training.trainingIndex)
                    .ToList();
                thisPeriod = dateTime.Year.ToString();
                dateTime = dateTime.AddYears(-1);
            }

            sets = TrainingModel.setInfoData.setsInfo
                    .Where(set => set.exerciseName == exercise.name)
                    .Where(set => trainings.Contains(set.trainingIndex))
                    .ToList();

            float setCount = sets.Count;
            float repsStats = -1f;
            float loadStats = -1f;
            float timeStats = -1f;
            string repsText = "-";
            string loadText = "-";
            string timeText = "-";

            //policz statystyki
            if (mode == 0)
            {
                //sum
                repsStats = (float)sets.Sum(set => set.reps);
                if (exercise.onReps)
                {
                    loadStats = sets.Sum(set => set.reps * set.load);
                    timeStats = (float)sets.Sum(set => set.reps * set.time);
                }
                else
                {
                    loadStats = sets.Sum(set => set.load);
                    timeStats = (float)sets.Sum(set => set.time);
                }
                repsText = repsStats.ToString("0");
                loadText = loadStats.ToString("0") +"kg";
                timeText = timeStats.ToString("0") + "s";    
            }
            else if(mode == 1 && setCount != 0f)
            {
                //average         
                float RepCount = (float)sets.Sum(set => set.reps);
                repsStats = RepCount / setCount;
                if (exercise.onReps && RepCount != 0f)
                {
                    loadStats = sets.Sum(set => set.reps * set.load) / RepCount;
                    timeStats = sets.Sum(set => set.reps * set.time) / RepCount;
                }
                else
                {
                    loadStats = sets.Sum(set => set.load) / setCount;
                    timeStats = sets.Sum(set => set.time) / setCount;
                }
                repsText = repsStats.ToString("0.0");
                loadText = loadStats.ToString("0.0") + "kg";
                timeText = timeStats.ToString("0.0") + "s";
            }

            else if (mode == 2 && sets.Count != 0f)
            {
                //max
                repsStats = (float)sets.Max(set => set.reps);
                loadStats = sets.Max(set => set.load);
                timeStats = (float)sets.Max(set => set.time);
                repsText = repsStats.ToString("0");
                loadText = loadStats.ToString("0.0") +"kg";
                timeText = timeStats.ToString("0") + "s";
            }

            //wstawienie prefab
            GameObject newPrefabInstance = Instantiate(exerciseStatsPrefab, exerciseStatsContent.transform);
            //widocznoœæ elementów prefab i edycja tekstów
            gameObject = newPrefabInstance.transform.Find("Period").gameObject;
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = thisPeriod;

            gameObject = newPrefabInstance.transform.Find("Sets").gameObject;
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            textComponent.text = setCount.ToString();

            gameObject = newPrefabInstance.transform.Find("Reps").gameObject;
            if (exercise.onReps)
            {
                textComponent = gameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = repsText;
            }
            else gameObject.SetActive(false);

            gameObject = newPrefabInstance.transform.Find("Load").gameObject;
            if (exercise.onLoad) 
            {
                textComponent = gameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = loadText;
            }
            else gameObject.SetActive(false);

            gameObject = newPrefabInstance.transform.Find("Time").gameObject;
            if (exercise.onTime) 
            {
                textComponent = gameObject.GetComponent<TextMeshProUGUI>();
                textComponent.text = timeText;
            }
            else gameObject.SetActive(false);
            //pozycja prefab
            rectTransform = newPrefabInstance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, yPosition);
            yPosition -= yDelta;

            //pierwszy trening tego æwiczenia, jeœli data wczeœniejsza to koñcz pêtla
            if (TrainingModel.isBeforeStartDate(dateTime.Day, dateTime.Month, dateTime.Year)) break;
        }

        //zwieksz content area
        rectTransform = exerciseStatsContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, -yPosition);

    }

    public void dropdownPeriodsChanged(int dropDown)
    {
        periods = dropDown;
        displayExerciseStatsFlag = true;
    }

    public void dropdownModeChanged(int dropDown)
    {
        mode = dropDown;
        displayExerciseStatsFlag = true;
    }


}
