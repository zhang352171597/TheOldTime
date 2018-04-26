using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONTotalSkill{
	#region 单例
	private static JSONTotalSkill instance;
	public static JSONTotalSkill getInstance(){
	if (instance == null)
		instance = new JSONTotalSkill ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONTotalSkill");
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
	/// 名称
	/// </summary>
	public int GetID(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["ID"].AsInt;
		return  value;
	}
	/// <summary>
	/// 名称 :From Index
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
	public int[] GetListType(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetType(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 技能类型
	/// </summary>
	public string GetType(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Type"].ToString();
		return  value;
	}
	/// <summary>
	/// 技能类型 :From Index
	/// </summary>
	public string GetType(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetType(index.ToString());
			return  value;
		}
		value = json["Type"].ToString();
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
	/// 技能名称
	/// </summary>
	public string GetName(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Name"].ToString();
		return  value;
	}
	/// <summary>
	/// 技能名称 :From Index
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
	public int[] GetListMapID(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetMapID(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 映射组
	/// </summary>
	public string GetMapID(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["MapID"].ToString();
		return  value;
	}
	/// <summary>
	/// 映射组 :From Index
	/// </summary>
	public string GetMapID(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetMapID(index.ToString());
			return  value;
		}
		value = json["MapID"].ToString();
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

	#endregion
}