using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColdGunControllrBase : GunControlBase
{

    private ColdGunViewBase m_ColdView;


    protected override void InitOther()
    {

        m_ColdView = (ColdGunViewBase)M_BaseView;
        CanShoot = false;
        m_ColdView.GunStar.sizeDelta = new Vector2(0, 0);
    }

    /// <summary>
    /// 冷兵器的射击控制
    /// </summary>
    protected override void HoldControl()
    {
        if(Input.GetMouseButtonDown(1))
        {
            m_ColdView.M_Animator.SetBool("HoldPose", true);
            m_ColdView.HoldState(1.0f);
        }
        if(Input.GetMouseButtonUp(1))
        {
            m_ColdView.NormalState(1.0f);
            m_ColdView.M_Animator.SetBool("HoldPose", false);
            CanShoot = false;
        }
    }






}
