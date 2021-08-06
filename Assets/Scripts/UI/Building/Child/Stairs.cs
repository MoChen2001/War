using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : BuildingBase
{

    protected override void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformForNormalStairs")
        {
            collObj = coll.gameObject;
            IsCanPut = true;
            IsAttach = true;
            Vector3 target = coll.gameObject.GetComponent<Transform>().parent.position;
            Vector3 offset = Vector3.zero;
            Vector3 quaternion = Vector3.zero;
            switch (coll.gameObject.name)
            {
                case "A":
                    offset = new Vector3(-2.5f, 0, 0);
                    quaternion = new Vector3(0,0,0);
                    break; 
                case "B":
                    offset = new Vector3(0, 0, 2.5f);
                    quaternion = new Vector3(0, 90, 0);
                    break;
                case "C":
                    offset = new Vector3(2.5f, 0, 0);
                    quaternion = new Vector3(0, 180, 0);
                    break;
                case "D":
                    offset = new Vector3(0, 0, -2.5f);
                    quaternion = new Vector3(0, 270, 0);
                    break;
                default:
                    break;
            }
            M_Transform.position = target + offset;
            M_Transform.rotation = Quaternion.Euler(quaternion);
        }
    }

    protected override void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "PlatformForNormalStairs")
        {
            collObj = null;
            IsCanPut = false;
            IsAttach = false;
        }
    }

}

