using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : BuildingBase
{



    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "WindowTrigger")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            M_Transform.position = coll.gameObject.transform.position;
            M_Transform.rotation = coll.gameObject.transform.rotation;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "WindowTrigger")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }
}
