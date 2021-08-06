using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManagers : MonoBehaviour 
{
    private Transform m_Transform;
    private Transform[] points;


    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        points = m_Transform.GetComponentsInChildren<Transform>();
        CreateAIManager();
    }



    /// <summary>
    ///  给生成点创建 AI 管理器
    /// </summary>
    private void CreateAIManager()
    {
        for(int i = 1; i < points.Length; i++)
        {
            if (i % 2 == 0)
            {
                points[i].gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.BOAR;
            }
            else
            {
                points[i].gameObject.AddComponent<AIManager>().AIManagerType = AIManagerType.CANNIBAL;
            }
        }
    }

}
