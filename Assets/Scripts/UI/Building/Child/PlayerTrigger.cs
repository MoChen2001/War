using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour 
{

	
	private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.name == "FPSController")
        {

            gameObject.transform.parent.GetComponent<DoorControl>().OpenDoor();
        }
    }



    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "FPSController")
        {
            gameObject.transform.parent.GetComponent<DoorControl>().CloseDoor();
        }
    }

}
