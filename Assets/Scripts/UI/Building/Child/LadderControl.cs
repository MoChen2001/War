using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderControl : MonoBehaviour 
{

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.name == "FPSController")
        {
            coll.gameObject.GetComponent<PlayerController>().Climp = true;
        }
    }


    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.name == "FPSController")
        {
            coll.gameObject.GetComponent<PlayerController>().Climp = false;
        }
    }
}
