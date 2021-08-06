using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
///  屋顶模型处理
/// </summary>
public class Roof : BuildingBase
{


    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "WallToRoof" || coll.gameObject.tag == "RoofToRoof")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            M_Transform.position = coll.gameObject.transform.position;
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "WallToRoof" || coll.gameObject.tag == "RoofToRoof")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }
}
