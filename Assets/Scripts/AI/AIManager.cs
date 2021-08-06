using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  AI模块总控制器
/// </summary>

public class AIManager : MonoBehaviour 
{


    private Transform m_Transform;
    private GameObject prefab_Cannibal;                         // 丧尸预制体
    private GameObject prefab_Boar;                             // 野猪预制体


    private AIManagerType aiManagerType = AIManagerType.NULL;   // AIManager的类型

    private Transform[] targetPos;                              // 巡逻点的位置存储数组
    private List<GameObject> aiList;                            // 管理生成AI的List
    private List<Vector3> targetPosList;                        // 目标点位置的数组


    public AIManagerType AIManagerType
    {
        get { return aiManagerType; }
        set { aiManagerType = value; }
    }

    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();


        prefab_Boar = Resources.Load<GameObject>("AI/Boar");
        prefab_Cannibal = Resources.Load<GameObject>("AI/Cannibal");

        InitOther();

        CreateAIByEnum();
    }

    
    private void InitOther()
    {
        aiList = new List<GameObject>();
        targetPosList = new List<Vector3>();
        targetPos = gameObject.GetComponentsInChildren<Transform>(true);

        for(int i = 1; i < targetPos.Length; i++)
        {
            targetPosList.Add(targetPos[i].position);
        }

    }


    #region 创建 AI相关方法

    /// <summary>
    /// 通过枚举类型创建AI
    /// </summary>
    private void CreateAIByEnum(int num = 5)
    {
        if (aiManagerType == AIManagerType.BOAR)
        {
            CreateAI(prefab_Boar,num);
        }
        else if (aiManagerType == AIManagerType.CANNIBAL)
        {
            CreateAI(prefab_Cannibal,num);
        }
    }



    /// <summary>
    ///  传入一个预制体，创建 AI
    /// </summary>
    /// <param name="obj"></param>
    private void CreateAI(GameObject obj,int num)
    {
        for(int i = 0; i < num; i++)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(obj,m_Transform.position,
                Quaternion.identity,m_Transform);
            temp.name = string.Format(obj.name + "_" + i);

            int index = Random.Range(0, targetPosList.Count);
            AI tempAI = temp.GetComponent<AI>();
            tempAI.M_AIType = aiManagerType;
            tempAI.Dir = targetPosList[index];
            tempAI.InitAI(100,20);
            aiList.Add(temp);
        }
    }




    /// <summary>
    ///  处理 AI 死亡的方法
    /// </summary>
    /// <param name="obj"></param>
    private void AIDeath(GameObject obj)
    {
        aiList.Remove(obj);
        StartCoroutine("CreateOneAIAfterAnyTimes", 3.0f);
    }




    /// <summary>
    ///  等待几秒 创建一个单独的 AI
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateOneAIAfterAnyTimes(float times)
    {
        yield return new WaitForSeconds(times);
        CreateAIByEnum(1);
    }

    #endregion



    /// <summary>
    ///  改变某一个 AI 的巡逻点的函数
    ///  这里的逻辑是，当 AI 判断出自己离自己目标点的位置很近的时候
    ///  向上层发送消息来调用这个消息，上层则为该 AI 重新寻找一个目标点
    /// </summary>
    public void ChangeAIDir(GameObject obj)
    {
        AI temp = obj.GetComponent<AI>();
        int index = Random.Range(0, targetPosList.Count);
        while(targetPosList[index] != temp.Dir)
        {
            temp.Dir = targetPosList[index];
        }
       
    }



}
