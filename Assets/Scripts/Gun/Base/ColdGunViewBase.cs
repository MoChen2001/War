using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColdGunViewBase : GunViewBase
{

    private GameObject prefab_Bullet;           // 子弹预制体


    public GameObject Prefab_Bullet { get { return prefab_Bullet; } set { prefab_Bullet = value; } }


    /// <summary>
    ///  冷兵器的默认状态
    /// </summary>
    public override void NormalState(float time = 0.2f,int FOV = 60)
    {
        base.NormalState(time, FOV);
        GunStar.sizeDelta = new Vector2(0, 0);
    }

    /// <summary>
    ///  冷兵器的开镜状态
    /// </summary>
    public override void HoldState(float time = 0.2f, int FOV = 45)
    {
        base.HoldState(time,FOV);
        GunStar.sizeDelta = new Vector2(256,256);
    }




}
