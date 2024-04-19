using UnityEngine;


//this class is used for jumping between pages of application
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
    public AssignObjects objects;

    public void Start()
    {
        Page11 = objects.Page11;
        Page12 = objects.Page12;
        Page21 = objects.Page21;
        Page22 = objects.Page22;
        Page23 = objects.Page23;
        Page31 = objects.Page31;
        Page32 = objects.Page32;

        currentPage = Page11;
    }


    // functions to jumping 
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
