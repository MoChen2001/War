using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpear : ColdGunControllrBase
{
    private WoodenSpearView m_View;


    protected override void InitOther()
    {
        base.InitOther();

        GunWeaponType = GunType.WoodenSpear;

        m_View = (WoodenSpearView)M_BaseView;
    }

    protected override void Shoot()
    {
        if (CanShoot && Input.GetMouseButtonDown(0) && Durable > 0)
        {
            CanShoot = false;
            m_View.NormalState(0.1f);
            m_View.M_Animator.SetTrigger("Fire");
            PlayAudio();
            GameObject temp = GameObject.Instantiate(m_View.Prefab_Bullet, m_View.ShootPos.position,
                m_View.ShootPos.rotation, m_View.Bullet_Parents);
            ArrowHelp tempArrowHelp = temp.GetComponent<ArrowHelp>();
            if (tempArrowHelp == null)
            {
                tempArrowHelp = temp.AddComponent<ArrowHelp>();
            }
            tempArrowHelp.Shoot(m_View.ShootPos.forward, 3000, Damage);
            Durable--;
            StartCoroutine(ShootHelp());
        }
    }


    private IEnumerator ShootHelp()
    {
        yield return new WaitForSeconds(2.0f);
        if (Input.GetMouseButton(1))
        {
            m_View.HoldState(0.1f);
            yield return new WaitForSeconds(0.1f);

        }
    }


    /// <summary>
    ///  用来改变是否可以射击的动画事件
    ///  动画播放时调用
    /// </summary>
    public void ChangeCanShoot(int val)
    {
        CanShoot = val == 1;
    }
}
