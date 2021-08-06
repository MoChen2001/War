using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuildingBase : MonoBehaviour 
{

    private Transform m_Transform;             // transform 组件
    private bool isCanPut = false;              // 可以摆放的字段
    private bool isNormal = false;             // 是否为默认状态
    private bool isAttach = false;             // 吸附状态的字段


    private Material oldMaterial;              // 默认材质球 
    private Material newMaterial;              // 透明材质球   

    protected GameObject collObj;

    public Transform M_Transform { get { return m_Transform; } }
    public bool IsCanPut { get { return isCanPut; } set { isCanPut = value; } }
    public bool IsAttach { get { return isAttach; } set { isAttach = value; } }


    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        newMaterial = Resources.Load<Material>("Building/Building Preview");

        oldMaterial = gameObject.GetComponent<MeshRenderer>().material;

        gameObject.GetComponent<MeshRenderer>().material = newMaterial;

    }




    private void Update()
    {
        if (!isNormal)
        {
            SetColor();
        }
    }


    /// <summary>
    ///  设置颜色的辅助函数
    /// </summary>
    private void SetColor()
    {
        if (isCanPut)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(0, 255, 0, 100);
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = new Color32(255, 0, 0, 100);
        }
    }



    /// <summary>
    ///  恢复默认状态
    /// </summary>
    public virtual void Normal()
    {
        isNormal = true;
        gameObject.GetComponent<MeshRenderer>().material = oldMaterial;
    }

    /// <summary>
    ///  销毁触发器
    /// </summary>
    public void DestoryTrigger()
    {
        if(collObj != null)
        {
            IsCanPut = false;
            GameObject.Destroy(collObj);
            if(gameObject.tag == "Platform")
            {
                Transform temp = gameObject.transform.Find(collObj.name[11].ToString());
                if(temp != null && temp.gameObject != null)
                {
                    GameObject.Destroy(temp.gameObject);
                }
            }
        }
    }


    /// <summary>
    ///  用于吸附的方法
    /// </summary>
    protected abstract void OnTriggerEnter(Collider coll);
    protected abstract void OnTriggerExit(Collider coll);


}
