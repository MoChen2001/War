using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletHelpBase : MonoBehaviour 
{

    /// <summary>
    ///  公共字段
    /// </summary>
    private int damage;
    private Transform m_Transform;
    private Rigidbody m_Rigidbody;




    /// <summary>
    ///  公共属性
    /// </summary>
    protected int Damage { get { return damage; }  set { damage = value; } } 
    protected Transform M_Transform { get { return m_Transform; } set { m_Transform = value; } }
    protected Rigidbody M_Rigidbody { get { return m_Rigidbody; } set { m_Rigidbody = value; } }



    #region 生命周期方法



    protected void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        m_Rigidbody = gameObject.GetComponent<Rigidbody>();
        if (m_Rigidbody == null)
        {
            m_Rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        if (gameObject.GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<Collider>();
        }
        m_Rigidbody.useGravity = false;

        InitOther();
    }


    private void OnCollisionEnter(Collision coll)
    {
        Collision(coll);
    }


    /// <summary>
    ///  销魂自身效果
    /// </summary>
    /// <returns></returns>
    protected IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Destroy(gameObject);
    }


    /// <summary>
    ///  尾部颤动效果
    /// </summary>
    protected IEnumerator AnimationPlay(Transform m_Pivot)
    {
        float stopTime = Time.time + 1.0f;
        float rangeTime = 1.0f;
        float vel = 0.0f;

        Quaternion startAngle = Quaternion.Euler(new Vector3(Random.Range(-5.0f, +5.0f), Random.Range(-5.0f, +5.0f), 0));

        while (rangeTime < stopTime)
        {
            Quaternion angle = Quaternion.Euler(new Vector3
                (Random.Range(rangeTime, rangeTime), Random.Range(-rangeTime, rangeTime), 0)) * startAngle;

            m_Pivot.transform.localRotation = angle;

            rangeTime = Mathf.SmoothDamp(rangeTime, 0, ref vel, stopTime - Time.time);

            yield return null;
        }
    }



    #endregion



    #region 虚拟方法
    /// <summary>
    ///  子类要实现的虚拟方法
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="force"></param>
    /// <param name="bulletDamage"></param>
    public abstract void Shoot(Vector3 dir, int force, int bulletDamage);
    protected abstract void Collision(Collision coll);
    protected abstract void InitOther();

    #endregion

}
