using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;



/// <summary>
/// 背包数据脚本.
/// </summary>
public class InventoryPanelModel : MonoBehaviour {


    /// <summary>
    /// 传递一个字符串，来读取其中的Json数据
    /// </summary>
    /// <param name="path">txt 文本的名称</param>
    /// <returns>返回一个 List<Inventory> </returns>
	public List<InventoryItem> GetItemList(string path)
    {
        return JsonLoadTool.LoadJsonWithPath<InventoryItem>("Json/" + path);
    }
	
}
