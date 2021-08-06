using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BuildingBase
{


    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformToWall")
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
        if (coll.gameObject.tag == "PlatformToWall")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }

}
