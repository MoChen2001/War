using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour 
{
    private Queue<GameObject> objPool;          // 对象池

    private void Awake()
    {
        objPool = new Queue<GameObject>();
        
    }


    /// <summary>
    /// 从对象池中取出一个对象
    /// 如果对象池为空就返回空对象
    /// </summary>
    /// <returns></returns>
    public GameObject GetObject()
    {
        GameObject temp = null;
        if(objPool.Count > 0)
        {
            temp =  objPool.Dequeue();
            temp.SetActive(true);
        }
        return temp;
    }


    /// <summary>
    /// 向对象池中添加对象
    /// </summary>
    /// <param name="go">要添加的对象的引用</param>
    /// <param name="time">添加的对象隐藏的时间</param>
    public void AddObject(GameObject go,float time = 1.0f)
    {
        StartCoroutine(DelayTimeAndHide(go, time));
    }




    /// <summary>
    /// 返回对象池是否为空的函数
    /// </summary>
    public bool PoolIsEmpty()
    {
        return objPool.Count == 0 ? true : false;
    }


    /// <summary>
    /// 辅助添加的函数
    /// </summary>
    /// <param name="go">添加的对象</param>
    /// <param name="time">延迟时间</param>
    private IEnumerator DelayTimeAndHide(GameObject go,float time)
    {
        yield return new WaitForSeconds(time);
        objPool.Enqueue(go);
        go.SetActive(false);
    }
}
