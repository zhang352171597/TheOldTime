using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONItem{
	#region 单例
	private static JSONItem instance;
	public static JSONItem getInstance(){
	if (instance == null)
		instance = new JSONItem ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONItem");
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
	public int[] GetListSuitID(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetSuitID(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 套装ID
	/// </summary>
	public int GetSuitID(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["SuitID"].AsInt;
		return  value;
	}
	/// <summary>
	/// 套装ID :From Index
	/// </summary>
	public int GetSuitID(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetSuitID(index.ToString());
			return  value;
		}
		value = json["SuitID"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListQuality(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetQuality(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 品质
	/// </summary>
	public int GetQuality(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Quality"].AsInt;
		return  value;
	}
	/// <summary>
	/// 品质 :From Index
	/// </summary>
	public int GetQuality(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetQuality(index.ToString());
			return  value;
		}
		value = json["Quality"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListSuperposition(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetSuperposition(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 是否叠加
	/// </summary>
	public int GetSuperposition(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Superposition"].AsInt;
		return  value;
	}
	/// <summary>
	/// 是否叠加 :From Index
	/// </summary>
	public int GetSuperposition(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetSuperposition(index.ToString());
			return  value;
		}
		value = json["Superposition"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListIcon(string target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetIcon(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 图标
	/// </summary>
	public string GetIcon(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Icon"].ToString();
		return  value;
	}
	/// <summary>
	/// 图标 :From Index
	/// </summary>
	public string GetIcon(int index){
		string value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetIcon(index.ToString());
			return  value;
		}
		value = json["Icon"].ToString();
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
	public int[] GetListMapID(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetMapID(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 模型映射ID
	/// </summary>
	public int GetMapID(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["MapID"].AsInt;
		return  value;
	}
	/// <summary>
	/// 模型映射ID :From Index
	/// </summary>
	public int GetMapID(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetMapID(index.ToString());
			return  value;
		}
		value = json["MapID"].AsInt;
		return  value;
	}

	#endregion
}