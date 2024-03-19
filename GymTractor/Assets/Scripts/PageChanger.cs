using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageChanger : MonoBehaviour
{
    public bool isTraining = false;
    public string currentPage = "Page11";

    public void showInCanvas(string name)
    {
        if (DataManager.invalidNameFlag) return;
        gameObject.transform.Find(name).gameObject.SetActive(true);
    }

    public void hideinCanvas(string name)
    {
        if (DataManager.invalidNameFlag) return;
        gameObject.transform.Find(name).gameObject.SetActive(false);
    }

    public void changeCurrentPageNum(string name)
    {
        if (DataManager.invalidNameFlag) return;
        currentPage = name;
    }

    // Bottom Bar ------------------------------------------
    public void goToStats()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page21");
        showInCanvas(currentPage);
    }

    public void goToTraining()
    {
        hideinCanvas(currentPage);
        if (isTraining) changeCurrentPageNum("Page12");
        else changeCurrentPageNum("Page11");
        showInCanvas(currentPage);
    }

    public void goToExercises()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page31");
        showInCanvas(currentPage);
    }

    //Trening ------------------------------------------------
    public void startTraining()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page12");
        isTraining = true;
        //funkcja zaczynaj¹ca trening
        showInCanvas(currentPage);
    }

    public void EndTraining()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page11");
        isTraining = false;
        //funkcja zapisuj¹ca nowe dane do pliku i zamykaj¹ca trening
        showInCanvas(currentPage);
    }

    public void showExerciseStats()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page23");
        //funkcja przygotywuj¹ca i wyœwietlaj¹ca statystyki konretnego æwiczenia
        showInCanvas(currentPage);
    }

    //Adding, saving exercise ----------------------------------------
    public void addNewExrcise()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page32");
        showInCanvas(currentPage);
    }

    public void saveNewExrcise()
    {
        //funkcja spradzaj¹ca czy nazwa jest nie powtarzaj¹ca siê i w³aœciwa
        //funkcja zamykaj¹ca tryb zapisywania (jeœli siê bêdzie chcia³o wyjœæ przed zapisywaniem wyskakuje okienko)
        //funkcja zapiszuj¹ca do bazy danych
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page31");
        showInCanvas(currentPage);
    }

    public void chooseExercise()
    {
        hideinCanvas(currentPage);
        changeCurrentPageNum("Page22");
        showInCanvas(currentPage);
        //odpalanie trybu wybierania æwiczniea
    }




}
