using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class PageChanger : MonoBehaviour
{
    public bool isTraining = false;
    public string currentPage = "Page11";


    public void showInCanvas(string name)
    {
        gameObject.transform.Find(name).gameObject.SetActive(true);
    }

    public void hideinCanvas(string name)
    {
        gameObject.transform.Find(name).gameObject.SetActive(false);
    }

    // Bottom Bar ------------------------------------------
    public void goToStats()
    {
        hideinCanvas(currentPage);
        currentPage = "Page21";
        showInCanvas(currentPage);
    }

    public void goToTraining()
    {
        hideinCanvas(currentPage);
        if (isTraining) currentPage = "Page12";
        else currentPage = "Page11";
        showInCanvas(currentPage);
    }

    public void goToExercises()
    {
        hideinCanvas(currentPage);
        currentPage = "Page31";
        showInCanvas(currentPage);
    }

    //Trening ------------------------------------------------
    public void startTraining()
    {
        hideinCanvas(currentPage);
        currentPage = "Page12";
        isTraining = true;
        //funkcja zaczynaj¹ca trening
        showInCanvas(currentPage);
    }

    public void EndTraining()
    {
        hideinCanvas(currentPage);
        currentPage = "Page11";
        isTraining = false;
        //funkcja zapisuj¹ca nowe dane do pliku i zamykaj¹ca trening
        showInCanvas(currentPage);
    }

    public void showExerciseStats()
    {
        hideinCanvas(currentPage);
        currentPage = "Page23";
        //funkcja przygotywuj¹ca i wyœwietlaj¹ca statystyki konretnego æwiczenia
        showInCanvas(currentPage);
    }

    //Adding, saving exercise ----------------------------------------
    public void addNewExrcise()
    {
        hideinCanvas(currentPage);
        currentPage = "Page32";
        showInCanvas(currentPage);
    }

    public void saveNewExrcise()
    {
        //funkcja spradzaj¹ca czy nazwa jest nie powtarzaj¹ca siê i w³aœciwa
        //funkcja zamykaj¹ca tryb zapisywania (jeœli siê bêdzie chcia³o wyjœæ przed zapisywaniem wyskakuje okienko)
        //funkcja zapiszuj¹ca do bazy danych
        hideinCanvas(currentPage);
        currentPage = "Page31";
        showInCanvas(currentPage);
    }

    public void chooseExercise()
    {
        hideinCanvas(currentPage);
        currentPage = "Page22";
        showInCanvas(currentPage);
        //odpalanie trybu wybierania æwiczniea
    }




}
