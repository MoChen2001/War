﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLight : BuildingBase 
{

    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "LightPos")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            M_Transform.position = coll.gameObject.transform.position;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.name == "LightPos")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }
}
