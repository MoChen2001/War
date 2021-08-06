using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapedStairs : BuildingBase
{


    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformToStairs")
        {
            collObj = coll.gameObject;
            M_Transform.position = coll.gameObject.transform.position;
            M_Transform.rotation = coll.gameObject.transform.rotation;
            IsCanPut = true;
            IsAttach = true;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformToStairs")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }

}


