using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : BuildingBase
{


    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "DoorTrigger")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            M_Transform.position = coll.gameObject.transform.parent.Find("DoorPos").position;
            M_Transform.rotation = coll.gameObject.transform.rotation;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "DoorTrigger")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }
}
