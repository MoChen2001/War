using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


/// <summary>
///  枪械工厂类
/// </summary>
public class GunFactory : MonoBehaviour 
{
    private Dictionary<string, GameObject> objectDic;               // 枪械字典，键是枪械名，值是枪械预制体
    private List<GunWeaponData> gunData;                            // 枪械数据数组

    private GameObject buildModel;
    private Transform m_Transform;
    public static GunFactory Instance;

    /// <summary>
    ///  实现单例
    /// </summary>
    private void Awake()
    {
        Instance = this;

        buildModel = Resources.Load<GameObject>("Gun/Prefabs/Building Plan");
        InitGunData();
    }


    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        LoadPreafab();
    }




    /// <summary>
    ///  加载预制体函数
    /// </summary>
    private void LoadPreafab()
    {
        objectDic = new Dictionary<string, GameObject>();
        GameObject[] objs = Resources.LoadAll<GameObject>("Gun/Prefabs/");
        for (int i = 0; i < objs.Length; i++)
        {
            objectDic.Add(objs[i].name,objs[i]);
        }
    }



    /// <summary>
    ///  初始化枪械的数组
    /// </summary>
    private void InitGunData()
    {
        gunData = new List<GunWeaponData>();
        gunData = JsonLoadTool.LoadJsonWithPath<GunWeaponData>("Json/GunWeaponData");
    }





    /// <summary>
    ///  创建枪械的函数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>

    public GameObject CreateGun(string str,InventoryItemController iic)
    {
        GameObject res = null;
        if(str == "Building")
        { 
            res = GameObject.Instantiate(buildModel, m_Transform);
            res.AddComponent<Building>();
            res.name = str;
        }
        else if (objectDic.TryGetValue(str, out res))
        {
            res = GameObject.Instantiate(objectDic[str], m_Transform);
            res.name = str;
            InitGun(res,iic);
        }


        return res;
    }



    /// <summary>
    ///  初始化枪械的函数
    ///  需要使用 Json 数据来进行初始化
    /// </summary>
    private void InitGun(GameObject obj, InventoryItemController iic)
    {
        GunControlBase gcb = obj.GetComponent<GunControlBase>();
        for(int i = 0; i < gunData.Count; i++)
        {
            if(gunData[i].Name == obj.name)
            {
                gcb.Id = gunData[i].Id;
                gcb.Damage = gunData[i].Damage;
                gcb.IIc = iic;
                gcb.MaxDurage = iic.Number * gunData[i].Durage;
                gcb.SignaleDurable = gunData[i].Durage;
                gcb.Durable = iic.Number * gunData[i].Durage;
            }
        }

    }


}
