using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;



/// <summary>
/// 合成类数据（Model）
/// </summary>
public class CraftingPanelModel : MonoBehaviour 
{
	private string[] tabName = { "Icon_House", "Icon_Weapon" };
	public string[] TabIconName { get { return tabName; } }


    private Dictionary<int, CraftingSlotContentItem>  contentItemDic = null;


    private void Awake()
    {
        contentItemDic = LoadContentItemDic("CraftingMapJsonData");
    }


    /// <summary>
    /// 根据名称读取有关合成选项卡的 json数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
	public List<List<CraftingContentItem>> GetContentList(string name)
    {
		List<List<CraftingContentItem>> tempList = new List<List<CraftingContentItem>>();

		string jsonStr = Resources.Load<TextAsset>("Json/" + name).text;
		JsonData jsonData = JsonMapper.ToObject(jsonStr);
        for (int i = 0; i < jsonData.Count; i++)
        {
            List<CraftingContentItem> jdList = new List<CraftingContentItem>();
            JsonData jd = jsonData[i]["Type"];
            for (int j = 0; j < jd.Count; j++)
            {
                jdList.Add(JsonMapper.ToObject<CraftingContentItem>(jd[j].ToJson()));
            }
            tempList.Add(jdList);
        }

        return tempList;

	}


    /// <summary>
    /// 预加载合成槽的数据
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>

    private Dictionary<int, CraftingSlotContentItem> LoadContentItemDic(string name)
    {
        Dictionary<int, CraftingSlotContentItem> temp = new Dictionary<int, CraftingSlotContentItem>();
        string str = Resources.Load<TextAsset>("Json/" + name).text;
        JsonData jsonData = JsonMapper.ToObject(str);
        for (int j = 0; j < jsonData.Count; j++)
        {
            JsonData jd = jsonData[j]["Type"];
            for (int i = 0; i < jd.Count; i++)
            {
                int id = int.Parse(jd[i]["MapId"].ToString());
                string content = jd[i]["MapContents"].ToString();
                string[] contents = content.Split(',');
                int count = int.Parse(jd[i]["MaterialsCount"].ToString());
                string mapName = jd[i]["MapName"].ToString();
                int haveBar = int.Parse(jd[i]["HaveBar"].ToString());
                CraftingSlotContentItem tempItem = new CraftingSlotContentItem(id, contents, count, mapName, haveBar);
                temp.Add(id, tempItem);
            }
        }

        return temp;
    }



    public CraftingSlotContentItem GetMapItemById(int id)
    {
        CraftingSlotContentItem temp = null;
        contentItemDic.TryGetValue(id, out temp);
        return temp;
    }

}
