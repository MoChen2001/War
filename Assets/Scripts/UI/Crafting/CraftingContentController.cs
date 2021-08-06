using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingContentController : MonoBehaviour {

	private Transform m_Transform;
    private int index;

    private CraftingContentItemController currentItem;

    


	private void Awake()
    {
        currentItem = null;
        m_Transform = gameObject.GetComponent<Transform>();
    }

    // 创建所有选项卡
    private void CreateContent(List<CraftingContentItem> contentList, GameObject prefab)
    {
        for (int i = 0; i < contentList.Count; i++)
        {
            GameObject temp = GameObject.Instantiate(prefab, m_Transform);
            temp.name = "content" + i;
            temp.GetComponent<CraftingContentItemController>().InitContentItem(contentList[i]);
        }
    }



    // 初始化该选项卡区域
    public void InitContent(int index, List<CraftingContentItem> contentList,GameObject prefab)
    {
        this.index = index;
        CreateContent(contentList, prefab);

    }



    /// <summary>
    /// 点击按钮时切换选项卡的函数
    /// </summary>
    /// <param name="go"></param>
    public void ResetContentItem(CraftingContentItemController go)
    {
        if(currentItem == go)
        {
            return;
        }
        if(currentItem != null)
        {
            currentItem.ItemNormalState();
        }
        currentItem = go;
        currentItem.ItemClickState();
        SendMessageUpwards("CreateSlotContent", go.ItemID);
    }
}
