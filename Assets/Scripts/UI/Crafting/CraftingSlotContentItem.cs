using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
///  中间的所有合成槽的数据实体类
/// </summary>
public class CraftingSlotContentItem  
{
    private int mapId;
    private string[] mapItem;
    private int materialsCount;
    private string mapResultItem;
    private int haveBar;

    public int MapId
    {
        get { return mapId; }
        set { mapId = value; }
    }

    public string[] MapContents
    {
        get { return mapItem; }
        set { mapItem = value; }
    }

    public int MaterialsCount
    {
        get { return materialsCount; }
        set { materialsCount = value; }
    }

    public string MapName
    {
        get { return mapResultItem; }
        set { mapResultItem = value; }
    }

    public int HaveBar
    {
        get { return haveBar; }
        set { haveBar = value; }
    }


    public CraftingSlotContentItem()
    {

    }

    public CraftingSlotContentItem(int id,string[] item,int count,string result,int haveBar)
    {
        this.mapId = id;
        mapItem = item;
        materialsCount = count;
        mapResultItem = result;
        this.haveBar = haveBar;
    }


    public override string ToString()
    {
        return string.Format(mapId + "--" + mapItem + "--" + materialsCount + "--" + mapResultItem + "--" + haveBar + "--");
    }
}
