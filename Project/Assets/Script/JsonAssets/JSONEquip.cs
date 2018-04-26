using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONEquip{
	#region 单例
	private static JSONEquip instance;
	public static JSONEquip getInstance(){
	if (instance == null)
		instance = new JSONEquip ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONEquip");
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
	/// 
	/// </summary>
	public int GetID(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["ID"].AsInt;
		return  value;
	}
	/// <summary>
	///  :From Index
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
	public int[] GetListAttribute(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetAttribute(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 属性
	/// </summary>
	public string GetAttribute(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Attribute"].ToString();
		return  value;
	}
	/// <summary>
	/// 属性 :From Index
	/// </summary>
	public string GetAttribute(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetAttribute(index.ToString());
			return  value;
		}
		value = json["Attribute"].ToString();
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListSkill(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetSkill(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 技能
	/// </summary>
	public string GetSkill(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Skill"].ToString();
		return  value;
	}
	/// <summary>
	/// 技能 :From Index
	/// </summary>
	public string GetSkill(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetSkill(index.ToString());
			return  value;
		}
		value = json["Skill"].ToString();
		return  value;
	}

	#endregion
}