using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using TMPro;
using System.Collections.Specialized;

public class TrainingManager : MonoBehaviour
{
    //<ExerciseIndex, SetCount>
    public static Dictionary<int, int> setNumbers = new Dictionary<int, int>();
    public static Dictionary<int, string> exerciseNames = new Dictionary<int, string>();
    public static int currentExerciseIndex;
    public static bool newExerciseFlag = false;
    public static Exercise currentExercise;
    public GameObject trainingTime;
    public GameObject excerciseName;
    public GameObject excerciseAndSet;
    public GameObject scrollViewContent;
    public GameObject prefabOneSet;

    public static int contentWidth = 961; 



    // Start is called before the first frame update
    void Start()
    {
        RectTransform rectTransform = scrollViewContent.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    void Update()
    {
        //clock updating

        //content œrodkuje siê

        //w razie potrzeby currentExerciseIndex aktualizuje siê i z nim inne rzeczy

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
        addNewExercise(exerciseNames[currentExerciseIndex], scrollViewContent, prefabOneSet, excerciseName, excerciseAndSet);
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
        if (setNumbers[currentExerciseIndex] == 0) return;
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

    public void removeExercise()
    {
        //trudniejsza funkcja ale trzeba j¹ zrobiæ
        //jeœli nie ma æwiczeñ
        if (setNumbers.Count == 0) return;
        //usuñ prefaby z odpowiedni¹ nazw¹
        foreach (Transform child in scrollViewContent.transform)
        {
            if (child.name.StartsWith(currentExerciseIndex.ToString()))
            {
                Destroy(child.gameObject);
            }
        }
        //zaaktualizuj s³owniki, aktualne æwiczenie, napisy, content area
        //jeœli to by³o ostatnie æwiczenie to przesuñ content position w lewo (chyba ¿e by³o pierwsze)
        //jeœli nie by³o ostatnie to przesuñ dalsze elementy w content w lewo
    }

    public void centerScroll(Vector2 contentPosition)
    {
        float centerTime = 0.3f;
        float startTime = Time.time;
        float timer = 0f;
        Vector2 destination = new Vector2(0f, 0f);

        if (contentPosition.x % contentWidth < 0.5f * contentWidth)
        {
            destination.x = contentPosition.x - (contentPosition.x % contentWidth);// to Left
        }
        else
        {
            destination.x = contentPosition.x + (contentWidth - (contentPosition.x % contentWidth));
        }
        while (timer < centerTime)
        {
            contentPosition = Vector2.Lerp(contentPosition, destination, timer/ centerTime);
            timer = Time.time - startTime;
        }
    }

    public void endTraining()
    {
        //to mo¿e byæ niez³e gówno
    }






}
