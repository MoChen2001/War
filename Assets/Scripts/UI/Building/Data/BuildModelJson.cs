using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;


public class BuildModelJson : MonoBehaviour 
{

	private Transform m_Transform;
	private List<Transform> allModelTransforms;


	// 场景中的模型得到的数据 List
	private List<BuildItem> modelList;
	// json 得到的数据 List
	private List<BuildItem> jsonList;


	private string jsonPath = null;

	private GameObject prefab_Model;


	private void Start()
    {
		m_Transform = gameObject.GetComponent<Transform>();

		allModelTransforms = new List<Transform>();
		modelList = new List<BuildItem>();
		jsonList = new List<BuildItem>();


		jsonPath = Application.dataPath + @"\Resources\Json\ModelJson.txt";


		JsonToObject();
	}


	private void OnDisable()
    {
		ObjectToJson();
    }


	/// <summary>
	///  游戏数据转化为 Json 数据
	/// </summary>
	private void ObjectToJson() 
	{
		modelList.Clear();

		for(int i = 0; i < m_Transform.childCount; i++)
        {
			allModelTransforms.Add(m_Transform.GetChild(i));
        }

		for(int i = 0; i < allModelTransforms.Count; i++)
        {
			Transform tra = allModelTransforms[i];
			BuildItem item = new BuildItem(tra.name, tra.position.x.ToString(), tra.position.y.ToString()
				, tra.position.z.ToString(),tra.rotation.x.ToString(), tra.rotation.y.ToString(), 
				tra.rotation.z.ToString(), tra.rotation.w.ToString());

			modelList.Add(item);
        }


		string str = JsonMapper.ToJson(modelList);


		File.Delete(jsonPath);
		StreamWriter sw = new StreamWriter(jsonPath);
		sw.Write(str);
		sw.Close();
	}


	/// <summary>
	///  json 数据转化为游戏数据
	/// </summary>
	private void JsonToObject()
    {
		jsonList.Clear();

		string str = File.ReadAllText(jsonPath);
		JsonData jsonData= JsonMapper.ToObject(str);

		for(int i = 0; i < jsonData.Count; i++)
        {
			BuildItem temp = JsonMapper.ToObject<BuildItem>(jsonData[i].ToJson());
			jsonList.Add(temp);
        }

		for(int i = 0; i < jsonList.Count; i++)
        {
			string name = jsonList[i].Name;
			Vector3 pos = new Vector3(float.Parse(jsonList[i].PosX), float.Parse(jsonList[i].PosY),
				float.Parse(jsonList[i].PosZ));
			Quaternion quat = new Quaternion(float.Parse(jsonList[i].RotX), float.Parse(jsonList[i].RotY),
				float.Parse(jsonList[i].RotZ), float.Parse(jsonList[i].RotW));

			prefab_Model = Resources.Load<GameObject>("Building/Prefabs/" + name);
			GameObject obj = GameObject.Instantiate(prefab_Model, pos, quat,m_Transform);



			/// 进行初始化
			obj.name = prefab_Model.name;
			obj.tag = prefab_Model.tag;

			if (obj.tag == "Wall") obj.layer = 14;
			else obj.layer = 13;

			obj.gameObject.GetComponent<BuildingBase>().Normal();
			GameObject.Destroy(obj.gameObject.GetComponent<BuildingBase>());
		}

    }
}
