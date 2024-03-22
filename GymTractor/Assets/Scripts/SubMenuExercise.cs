using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class SubMenuExercise : MonoBehaviour
{

    // Start is called before the first frame update
    public void SubMenuStart()
    {
        string title = gameObject.transform.parent.gameObject.name;
        DataManager.SubMenuStart(title);
    }

}
