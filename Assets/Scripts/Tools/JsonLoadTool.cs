using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public sealed class JsonLoadTool
{
    public static List<T> LoadJsonWithPath<T>(string path)
    {
        List<T> ts = new List<T>();
        string str = Resources.Load<TextAsset>(path).text;

        JsonData jsonData = JsonMapper.ToObject(str);

        for (int i = 0; i < jsonData.Count; i++)
        {
            T ii = JsonMapper.ToObject<T>(jsonData[i].ToJson());
            ts.Add(ii);
        }

        return ts;
    }



    public static List<T> LoadJsonWithPathUseIO<T>(string fileName)
    {
        List<T> ts = new List<T>();
        string str = File.ReadAllText(Application.dataPath + @"\Resources\Json\" + fileName);

        JsonData jsonData = JsonMapper.ToObject(str);

        for (int i = 0; i < jsonData.Count; i++)
        {
            T ii = JsonMapper.ToObject<T>(jsonData[i].ToJson());
            ts.Add(ii);
        }

        return ts;
    }

}
