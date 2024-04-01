using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Collections.Specialized;
using System.Timers;
using System.Globalization;

public class TrainingManager : MonoBehaviour
{
    //<ExerciseIndex, SetCount>
    public static Dictionary<int, int> setNumbers = new Dictionary<int, int>();
    public static Dictionary<int, string> exerciseNames = new Dictionary<int, string>();
    public static int currentExerciseIndex = 0;
    public static bool newExerciseFlag = false;
    public static Exercise currentExercise;
    public GameObject trainingTime;
    public GameObject exerciseName;
    public GameObject exerciseAndSet;
    public GameObject scrollViewContent;
    public GameObject prefabOneSet;
    public static bool scrollingFlag = false;
    public Vector2 currentContentPosition;
    public GameObject scrollView;
    public static int contentWidth;//do optymalizacji
    public static bool isTraining = false;
    public static bool isTrainingStart = false;
    public static float startTime;
    public static float timer;
    public int timerSeconds;
    public GameObject SubMenuObject;
    public GameObject labelSet;
    public GameObject labelReps;
    public GameObject labelLoad;
    public GameObject labelTime;
    public GameObject labelDone;
    


    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
        contentWidth = (int)((Screen.width) * (1920f/Screen.height)) - 100;
    }

    void Update()
    {
        if (isTrainingStart)
        {
            isTrainingStart = false;
            startTime = Time.realtimeSinceStartup;//bedzie dzia³a³o nawet w backroundzie
            timerSeconds = 0;
            isTraining = true;
        }
        //clock updating
        if (isTraining)
        {
            timer = Time.realtimeSinceStartup - startTime;
            if ((int)timer != timerSeconds)
            {
                timerSeconds = (int)timer;
                int seconds = timerSeconds % 60;
                int minutes = (timerSeconds/60) % 60;
                int hours = timerSeconds / 3600;
                displayTime(seconds, minutes, hours);
            }
        }

        if (newExerciseFlag)
        {
            newExerciseFlag = false;
            addNewExercise();
        }

    }

    static void addNewExercise(string exerciseName, GameObject scrollViewContent, GameObject prefabOneSet, GameObject excerciseNameObject, GameObject excerciseAndSet)
    {
        //nazwa cwiczenia
        TextMeshProUGUI textComponent = excerciseNameObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = exerciseName;
        //cwiczenie/set
        textComponent = excerciseAndSet.GetComponent<TextMeshProUGUI>();
        textComponent.text = currentExerciseIndex.ToString() + "/" + setNumbers.Count.ToString();
        //poszerz content area
        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x + contentWidth, rectTransform.sizeDelta.y);
        //przesuñ content do nowego æwiczenia
        rectTransform.anchoredPosition = new Vector2(-(currentExerciseIndex - 1) * contentWidth, rectTransform.anchoredPosition.y);
        //dodaj prefab
        GameObject newPrefabInstance = Instantiate(prefabOneSet, scrollViewContent.transform);
        newPrefabInstance.name = currentExerciseIndex.ToString() + " 1";
        rectTransform = newPrefabInstance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (currentExerciseIndex - 1) * contentWidth, rectTransform.anchoredPosition.y);
        rectTransform.sizeDelta = new Vector2(contentWidth , rectTransform.sizeDelta.y);
        //dostosuj ten prefab (niektóre rzeczy trzeba mo¿e wygasiæ)
        if (!currentExercise.onReps)
        {
            newPrefabInstance.transform.Find("Reps").gameObject.SetActive(false);
        }
        if (!currentExercise.onLoad)
        {
            newPrefabInstance.transform.Find("Load").gameObject.SetActive(false);
        }
        if (!currentExercise.onTime)
        {
            newPrefabInstance.transform.Find("Time").gameObject.SetActive(false);
        }
    }

    public void addNewExercise()
    {
        addNewExercise(exerciseNames[currentExerciseIndex], scrollViewContent, prefabOneSet, exerciseName, exerciseAndSet);
        adjustSetLabel();
    }

    public void addNewSet()
    {
        //zabezpieczenie przed b³êdami
        if (currentExercise == null) return;
        if (setNumbers[currentExerciseIndex] >= 9)
        {
            setNumbers[setNumbers.Count + 1] = 1;
            exerciseNames[exerciseNames.Count + 1] = currentExercise.name;
            currentExerciseIndex = setNumbers.Count;
            addNewExercise();
            return;
        }
        //pobierz wartoœæ currentIndex
        setNumbers[currentExerciseIndex] += 1;
        //dodaj prefab i wstaw prefab w odpowiednie miejsce
        GameObject newPrefabInstance = Instantiate(prefabOneSet, scrollViewContent.transform);
        newPrefabInstance.name = currentExerciseIndex.ToString() + " " + setNumbers[currentExerciseIndex].ToString();
        RectTransform rectTransform = newPrefabInstance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + (currentExerciseIndex - 1) * contentWidth, rectTransform.anchoredPosition.y - 100 * (setNumbers[currentExerciseIndex]-1));
        rectTransform.sizeDelta = new Vector2(contentWidth, rectTransform.sizeDelta.y);
        //dostosuj ten prefab
        TextMeshProUGUI textComponent = newPrefabInstance.transform.Find("Set").gameObject.GetComponent<TextMeshProUGUI>();
        textComponent.text = setNumbers[currentExerciseIndex].ToString() + ".";
        if (!currentExercise.onReps)
        {
            newPrefabInstance.transform.Find("Reps").gameObject.SetActive(false);
        }
        if (!currentExercise.onLoad)
        {
            newPrefabInstance.transform.Find("Load").gameObject.SetActive(false);
        }
        if (!currentExercise.onTime)
        {
            newPrefabInstance.transform.Find("Time").gameObject.SetActive(false);
        }
    }

    public void removeSet()
    {
        if (setNumbers.Count == 0 || setNumbers[currentExerciseIndex] < 1) return;
        //pobierz wartoœæ currentIndex i ilosc serii
        string toRemove = currentExerciseIndex.ToString() + " " + setNumbers[currentExerciseIndex].ToString();
        setNumbers[currentExerciseIndex] -= 1;
        //usun prefab z odpowiedni¹ nazw¹
        foreach (Transform child in scrollViewContent.transform)
        {
            if (child.name == toRemove)
            {
                Destroy(child.gameObject);
            }        
        }
    }

    public void saveAllTraining()
    {
        //termin treningu
        // Get the current date and time
        System.DateTime now = System.DateTime.Now;
        int weekOfYear = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(now, CalendarWeekRule.FirstDay, System.DayOfWeek.Monday);
        //duration of the training
        int minutes = (timerSeconds / 60) % 60;
        //saving training
        TrainingModel.addNewTrainingData(now.Day, weekOfYear, now.Month, now.Year, minutes);
        //index of the training

        for (int i = 1; i <= setNumbers.Count; i++)
        {
            for (int j = 1; j <= setNumbers[i]; j++)
            {
                //default values
                float load = -1f;
                int reps = -1;
                int time = -1;
                //getting inputs
                string current  = i.ToString() + " " + j.ToString();
                GameObject setGameObject = scrollViewContent.transform.Find(current).gameObject;
                GameObject gameObject = setGameObject.transform.Find("Reps").gameObject;
                if (gameObject != null)
                {
                    TMP_InputField inputField = gameObject.GetComponent<TMP_InputField>();
                    string inputText = inputField.text;
                    if (!string.IsNullOrEmpty(inputText) && !inputText.StartsWith("-"))
                    {
                        reps = int.Parse(inputText);
                    }
                }
                gameObject = setGameObject.transform.Find("Load").gameObject;
                if (gameObject != null)
                {
                    TMP_InputField inputField = gameObject.GetComponent<TMP_InputField>();
                    string inputText = inputField.text;
                    if (!string.IsNullOrEmpty(inputText) && !inputText.StartsWith("-") && inputText != "," && inputText != ".")
                    {
                        load = float.Parse(inputText);
                    }
                }
                gameObject = setGameObject.transform.Find("Time").gameObject;
                if (gameObject != null)
                {
                    TMP_InputField inputField = gameObject.GetComponent<TMP_InputField>();
                    string inputText = inputField.text;
                    if (!string.IsNullOrEmpty(inputText) && !inputText.StartsWith("-"))
                    {
                        time = int.Parse(inputText);
                    }
                }
                TrainingModel.addNewSetInfoData(exerciseNames[i], load, reps, time);
            }
        }
    }

    public void removeAllSets()
    {
        setNumbers.Clear();
        exerciseNames.Clear();
        currentExerciseIndex = 0;
        currentExercise = null;

        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);

        foreach (Transform child in scrollViewContent.transform)
        {
            Destroy(child.gameObject);
        }

        TextMeshProUGUI textComponent = exerciseName.GetComponent<TextMeshProUGUI>();
        textComponent.text = "Add first exercise";
        textComponent = exerciseAndSet.GetComponent<TextMeshProUGUI>();
        textComponent.text = "0/0";
    }

    public void removeExercise()
    {
        
        //jeœli nie ma æwiczeñ
        if (setNumbers.Count == 0) return;
        //zastanów siê nad innymi warunkami skoñczenia tej funkcji

        RectTransform rectTransform;
        foreach (Transform child in scrollViewContent.transform)
        {
            //usuñ prefaby z odpowiedni¹ nazw¹
            if (child.name.StartsWith(currentExerciseIndex.ToString()))
            {
                Destroy(child.gameObject);
            }

            int exeNr = (int)char.GetNumericValue(child.name[0]);
            //przesuñ dalsze prefaby w lewo i zmieñ ich nazwê
            if (exeNr > currentExerciseIndex)
            {
                rectTransform = child.gameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - contentWidth, rectTransform.anchoredPosition.y);
                child.name = (exeNr-1).ToString() + child.name.Substring(1);
            }
        }

        //zaaktualizuj s³owniki, aktualne æwiczenie
        for (int i = currentExerciseIndex; i < setNumbers.Count; i++)
        {
            setNumbers[i] = setNumbers[i + 1];
            exerciseNames[i] = exerciseNames[i + 1];
        }
        if (currentExerciseIndex == exerciseNames.Count)
        {
            currentExerciseIndex -= 1;
        }
        setNumbers.Remove(setNumbers.Count);
        exerciseNames.Remove(exerciseNames.Count);
        //napisy
        if (currentExerciseIndex == 0)
        {
            TextMeshProUGUI textComponent = exerciseName.GetComponent<TextMeshProUGUI>();
            textComponent.text = "Add first exercise";
            textComponent = exerciseAndSet.GetComponent<TextMeshProUGUI>();
            textComponent.text = "0/0";
        }
        else
        {
            TextMeshProUGUI textComponent = exerciseName.GetComponent<TextMeshProUGUI>();
            textComponent.text = exerciseNames[currentExerciseIndex];
            textComponent = exerciseAndSet.GetComponent<TextMeshProUGUI>();
            textComponent.text = currentExerciseIndex.ToString()+"/"+setNumbers.Count.ToString();
        }
        //content area
        rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x - contentWidth, rectTransform.sizeDelta.y);
        rectTransform.anchoredPosition = new Vector2(-(currentExerciseIndex - 1) * contentWidth, rectTransform.anchoredPosition.y);

        adjustSetLabel();
        subMenuEnd();
    }



    public void goToNext()
    {
        //jesli to ostatnie cwiczenie to nic nie rób
        if (currentExerciseIndex == setNumbers.Count) return;

        currentExerciseIndex += 1;
        currentExercise = ExerciseManager.exerciseData.exercises.Find(exercise => exercise.name.ToLower() == exerciseNames[currentExerciseIndex].ToLower());

        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x - contentWidth, rectTransform.anchoredPosition.y);

        TextMeshProUGUI textComponent = exerciseName.GetComponent<TextMeshProUGUI>();
        textComponent.text = exerciseNames[currentExerciseIndex];
        textComponent = exerciseAndSet.GetComponent<TextMeshProUGUI>();
        textComponent.text = currentExerciseIndex.ToString() + "/" + setNumbers.Count.ToString();

        adjustSetLabel();
    }

    public void goToPrevious()
    {
        if (currentExerciseIndex < 2) return;

        currentExerciseIndex -= 1;
        currentExercise = ExerciseManager.exerciseData.exercises.Find(exercise => exercise.name.ToLower() == exerciseNames[currentExerciseIndex].ToLower());

        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + contentWidth, rectTransform.anchoredPosition.y);

        TextMeshProUGUI textComponent = exerciseName.GetComponent<TextMeshProUGUI>();
        textComponent.text = exerciseNames[currentExerciseIndex];
        textComponent = exerciseAndSet.GetComponent<TextMeshProUGUI>();
        textComponent.text = currentExerciseIndex.ToString() + "/" + setNumbers.Count.ToString();

        adjustSetLabel();
    }

    public void displayTime(int seconds, int minutes, int hours)
    {
        string s, min, h;
        if (seconds < 10)
        {
            s = "0" + seconds.ToString();
        }
        else
        {
            s = seconds.ToString();
        }
        if (minutes < 10)
        {
            min = "0" + minutes.ToString();
        }
        else
        {
            min = minutes.ToString();
        }
        if (hours < 10)
        {
            h = "0" + hours.ToString();
        }
        else
        {
            h = hours.ToString();
        }
        TextMeshProUGUI textComponent = trainingTime.GetComponent<TextMeshProUGUI>();
        textComponent.text = h + ":" + min + ":" + s;
    }

    public void subMenuStart()
    {
        if (currentExerciseIndex == 0) return;
        SubMenuObject.SetActive(true);
    }

    public void subMenuEnd()
    {
        SubMenuObject.SetActive(false);
    }

    public void adjustSetLabel()
    {
        if (currentExerciseIndex > 0)
        {
            labelSet.SetActive(true);
            labelDone.SetActive(true);
        }
        else 
        {
            labelSet.SetActive(false);
            labelDone.SetActive(false);
            labelReps.SetActive(false);
            labelLoad.SetActive(false);
            labelTime.SetActive(false);
            return;
        }

        if (currentExercise.onReps == true) labelReps.SetActive(true);
        else labelReps.SetActive(false);

        if (currentExercise.onLoad == true) labelLoad.SetActive(true);
        else labelLoad.SetActive(false);

        if (currentExercise.onTime == true) labelTime.SetActive(true);
        else labelTime.SetActive(false);
    }



}
