using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paltform : BuildingBase
{


    private void Start()
    {
        IsCanPut = true;
    }


    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag != "Terrain")
        {
            IsCanPut = false;
        }
    }


    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag != "Terrain")
        {
            IsCanPut = false;
        }
    }

    private void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag != "Terrain" )
        {
            IsCanPut = true;
        }

    }



    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformForPlatform")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            Vector3 target = coll.gameObject.GetComponent<Transform>().position;
            M_Transform.position = target;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformForPlatform")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }

}
