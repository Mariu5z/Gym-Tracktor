using UnityEngine;

//this class is used in prefabs in dynamic lists to obtain information of this specific prefab and use it in application
public class SubMenuExercise : MonoBehaviour
{
    public string title;

    //opening subMenu in Page 31
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

    public void chooseExerciseForStats()
    {
        title = gameObject.transform.parent.gameObject.name;
        PageChanger.goToExerciseStats();
        Statistics.currentExercise = title;
        Statistics.displayExerciseStatsFlag = true;
    }

}
