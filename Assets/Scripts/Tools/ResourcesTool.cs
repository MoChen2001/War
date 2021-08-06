using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class ResourcesTool 
{
    /// <summary>
    /// 根据路径加载其中的所有 Sprite 资源
    /// </summary>
    /// <param name="path">路径</param>
    /// <returns>返回以名称为键，Sprite 为值的字典</returns>
    public static Dictionary<string,Sprite> LoadSpriteWithPath(string path)
    {
        Dictionary<string, Sprite> temp = new Dictionary<string, Sprite>();
        Sprite[] itemSprites = Resources.LoadAll<Sprite>(path);
        for (int i = 0; i < itemSprites.Length; i++)
        {
            temp.Add(itemSprites[i].name, itemSprites[i]);
        }
        return temp;
    }


    /// <summary>
    /// 根据名称获得字典中的 sprite
    /// </summary>
    public static Sprite GetSpriteWithName(string name, Dictionary<string, Sprite> dic)
    {
        Sprite sprite = null;

        dic.TryGetValue(name,out sprite);

        return sprite;
    }

}
