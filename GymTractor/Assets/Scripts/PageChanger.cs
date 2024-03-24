using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PageChanger : MonoBehaviour
{
    public static GameObject currentPage;
    public static GameObject Page11;
    public static GameObject Page12;
    public static GameObject Page21;
    public static GameObject Page22;
    public static GameObject Page23;
    public static GameObject Page31;
    public static GameObject Page32;

    public void Start()
    {
        Page11 = GameObject.Find("Page11");
        currentPage = Page11;

        Page12 = GameObject.Find("Page12");
        Page12.SetActive(false);

        Page21 = GameObject.Find("Page21");
        Page21.SetActive(false);

        Page22 = GameObject.Find("Page22");
        Page22.SetActive(false);

        Page23 = GameObject.Find("Page23");
        Page23.SetActive(false);

        Page31 = GameObject.Find("Page31");
        Page31.SetActive(false);

        Page32 = GameObject.Find("Page32");
        Page32.SetActive(false);
    }


    // Bottom Bar ------------------------------------------
    public static void goToStats()
    {
        currentPage.SetActive(false);
        currentPage = Page21;
        currentPage.SetActive(true);
    }

    public static void goToTraining()
    {
        currentPage.SetActive(false);
        if (TrainingManager.isTraining) currentPage = Page12;
        else currentPage = Page11;
        currentPage.SetActive(true);
    }

    public static void goToExercises()
    {
        currentPage.SetActive(false);
        currentPage = Page31;
        currentPage.SetActive(true);
    }

    //Trening ------------------------------------------------
    public static void startTraining()
    {
        currentPage.SetActive(false);
        currentPage = Page12;
        TrainingManager.isTrainingStart = true;
        //funkcja zaczynaj¹ca trening
        currentPage.SetActive(true);
    }

    public static void EndTraining()
    {
        currentPage.SetActive(false);
        currentPage = Page11;
        TrainingManager.isTraining = false;
        //funkcja zapisuj¹ca nowe dane do pliku i zamykaj¹ca trening
        currentPage.SetActive(true);
    }

    public static void goToExerciseStats()
    {
        currentPage.SetActive(false);
        currentPage = Page23;
        //funkcja przygotywuj¹ca i wyœwietlaj¹ca statystyki konretnego æwiczenia
        currentPage.SetActive(true);
    }

    //Adding, saving exercise ----------------------------------------
    public static void goAddNewExrcise()
    {
        currentPage.SetActive(false);
        currentPage = Page32;
        currentPage.SetActive(true);
    }

    public static void goChooseExercise()
    {
        currentPage.SetActive(false);
        currentPage = Page22;
        currentPage.SetActive(true);
    }




}
