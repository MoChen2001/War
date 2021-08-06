using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour 
{

    private void OnEnable()
    {
        InputManager.Instance.BuildPanelControl = true;
    }


    private void OnDisable()
    {
        InputManager.Instance.BuildPanelControl = false;
    }

}
