using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildItemControl : MonoBehaviour
{

    private Transform m_Transform;
    private Image item_BG;              // 该项的背景


    private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        item_BG = gameObject.GetComponent<Image>();
    }


    /// <summary>
    ///  初始化 Item 的函数
    ///  主要进行资源的设置等等
    /// </summary>
    public void Init(int index, Sprite sprite)
    {
        gameObject.name = string.Format("Item_" + index);
        m_Transform.rotation = Quaternion.Euler(new Vector3(0, 0, index * 40));

        Transform iconTransform = m_Transform.Find("Icon").GetComponent<Transform>();

        if (sprite == null)
        {
            iconTransform.GetComponent<Image>().enabled = false;
        }
        else
        {
            iconTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            iconTransform.GetComponent<Image>().sprite = sprite;
            item_BG.enabled = false;
        }
    }



    /// <summary>
    ///  显示背景
    /// </summary>
    public void ShowBG()
    {
        item_BG.enabled = true;
    }

    /// <summary>
    ///  隐藏背景
    /// </summary>
    public void HideBG()
    {
        item_BG.enabled = false;
    }

}
