using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


/// <summary>
/// 背包数据脚本.
/// </summary>
public class InventoryPanelModel : MonoBehaviour {


    /// <summary>
    /// 传递一个字符串，来读取其中的Json数据
    /// </summary>
    /// <param name="path">txt 文本的名称</param>
    /// <returns>返回一个 List<Inventory> </returns>
	public List<InventoryItem> GetItemList(string fileName)
    {
        //return JsonLoadTool.LoadJsonWithPath<InventoryItem>("Json/" + path);
        return JsonLoadTool.LoadJsonWithPathUseIO<InventoryItem>(fileName);
    }




    public void ObjectToJson(List<GameObject> inventoryItems,string fileName)
    {
        List<InventoryItem> target = new List<InventoryItem>();
        for(int i = 0; i < inventoryItems.Count; i++)
        {
            Transform tempTransform = inventoryItems[i].transform.Find("InventoryItem");
            InventoryItem tempI = null;
            if(tempTransform != null)
            {
                InventoryItemController iic = tempTransform.GetComponent<InventoryItemController>();
                if(iic != null)
                {
                    tempI = new InventoryItem(iic.ItemID, iic.ImageName, iic.Number,iic.ItemBar);
                }
                else
                {
                    tempI = new InventoryItem(0,"",0,0);
                }
            }
            else
            {
                tempI = new InventoryItem(0, "", 0, 0);
            }
            target.Add(tempI);
        }

        string str = JsonMapper.ToJson(target);

        File.Delete(Application.dataPath + @"\Resources\Json\" + fileName);
        StreamWriter sw = new StreamWriter(Application.dataPath + @"\Resources\Json\" + fileName);
        sw.Write(str);
        sw.Close();
    }
	
}
