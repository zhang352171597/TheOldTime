using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONMap{
	#region 单例
	private static JSONMap instance;
	public static JSONMap getInstance(){
	if (instance == null)
		instance = new JSONMap ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONMap");
			return json;
		}
	}
	#endregion
	#region 外部调用
	public void Clear(){
		json = null;
	}
	public JSONNode GetJSON(string key){
		return JSONData [key];
	}
	public JSONNode GetJSON(int index){
		return JSONData [index];
	}
	/// <summary>
	/// 有多少个数据
	/// </summary>
	public int GetCount(){
	return JSONData.Count;
	}

    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListID(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetID(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// ID
	/// </summary>
	public int GetID(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["ID"].AsInt;
		return  value;
	}
	/// <summary>
	/// ID :From Index
	/// </summary>
	public int GetID(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetID(index.ToString());
			return  value;
		}
		value = json["ID"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListName(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetName(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 名称
	/// </summary>
	public string GetName(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Name"].ToString();
		return  value;
	}
	/// <summary>
	/// 名称 :From Index
	/// </summary>
	public string GetName(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetName(index.ToString());
			return  value;
		}
		value = json["Name"].ToString();
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListType(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetType(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 类型
	/// </summary>
	public int GetType(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Type"].AsInt;
		return  value;
	}
	/// <summary>
	/// 类型 :From Index
	/// </summary>
	public int GetType(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetType(index.ToString());
			return  value;
		}
		value = json["Type"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListInfo(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetInfo(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 说明
	/// </summary>
	public string GetInfo(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Info"].ToString();
		return  value;
	}
	/// <summary>
	/// 说明 :From Index
	/// </summary>
	public string GetInfo(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetInfo(index.ToString());
			return  value;
		}
		value = json["Info"].ToString();
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListMonsterInfo(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetMonsterInfo(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 怪物信息
	/// </summary>
	public string GetMonsterInfo(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["MonsterInfo"].ToString();
		return  value;
	}
	/// <summary>
	/// 怪物信息 :From Index
	/// </summary>
	public string GetMonsterInfo(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetMonsterInfo(index.ToString());
			return  value;
		}
		value = json["MonsterInfo"].ToString();
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListDrop(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetDrop(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 地图专属掉落
	/// </summary>
	public string GetDrop(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Drop"].ToString();
		return  value;
	}
	/// <summary>
	/// 地图专属掉落 :From Index
	/// </summary>
	public string GetDrop(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetDrop(index.ToString());
			return  value;
		}
		value = json["Drop"].ToString();
		return  value;
	}

	#endregion
}