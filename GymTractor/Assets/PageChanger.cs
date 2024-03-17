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
        //funkcja zaczynaj�ca trening
        showInCanvas(currentPage);
    }

    public void EndTraining()
    {
        hideinCanvas(currentPage);
        currentPage = "Page11";
        isTraining = false;
        //funkcja zapisuj�ca nowe dane do pliku i zamykaj�ca trening
        showInCanvas(currentPage);
    }

    public void showExerciseStats()
    {
        hideinCanvas(currentPage);
        currentPage = "Page23";
        //funkcja przygotywuj�ca i wy�wietlaj�ca statystyki konretnego �wiczenia
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
        //funkcja spradzaj�ca czy nazwa jest nie powtarzaj�ca si� i w�a�ciwa
        //funkcja zamykaj�ca tryb zapisywania (je�li si� b�dzie chcia�o wyj�� przed zapisywaniem wyskakuje okienko)
        //funkcja zapiszuj�ca do bazy danych
        hideinCanvas(currentPage);
        currentPage = "Page31";
        showInCanvas(currentPage);
    }

    public void chooseExercise()
    {
        hideinCanvas(currentPage);
        currentPage = "Page22";
        showInCanvas(currentPage);
        //odpalanie trybu wybierania �wiczniea
    }




}
