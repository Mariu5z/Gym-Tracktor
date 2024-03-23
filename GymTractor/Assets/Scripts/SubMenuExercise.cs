using System.Collections;
using System.Collections.Generic;
// System.Diagnostics;

//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuExercise : MonoBehaviour
{
    public string title;

    public void SubMenuStart()
    {
        title = gameObject.transform.parent.gameObject.name;
        DataManager.SubMenuStart(title);
    }

    public void chooseNewExerciseInTraining()
    {
        title = gameObject.transform.parent.gameObject.name;
        TrainingManager.setNumbers[TrainingManager.setNumbers.Count + 1] = 1;
        TrainingManager.exerciseNames[TrainingManager.exerciseNames.Count + 1] = title;
        TrainingManager.currentExerciseIndex = TrainingManager.setNumbers.Count;
        TrainingManager.currentExercise = ExerciseManager.exerciseData.exercises.Find(exercise => exercise.name.ToLower() == title.ToLower());
        TrainingManager.newExerciseFlag = true;
        PageChanger.goToTraining();
    }

}
