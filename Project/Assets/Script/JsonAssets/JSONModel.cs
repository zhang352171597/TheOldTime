using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONModel{
	#region 单例
	private static JSONModel instance;
	public static JSONModel getInstance(){
	if (instance == null)
		instance = new JSONModel ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONModel");
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
	public int[] GetListScale(float target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetScale(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 缩放
	/// </summary>
	public float GetScale(string key){
		JSONNode json = GetJSON (key);
		float  value;
		value = json["Scale"].AsFloat;
		return  value;
	}
	/// <summary>
	/// 缩放 :From Index
	/// </summary>
	public float GetScale(int index){
		float value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetScale(index.ToString());
			return  value;
		}
		value = json["Scale"].AsFloat;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListATT(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetATT(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 物攻
	/// </summary>
	public int GetATT(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["ATT"].AsInt;
		return  value;
	}
	/// <summary>
	/// 物攻 :From Index
	/// </summary>
	public int GetATT(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetATT(index.ToString());
			return  value;
		}
		value = json["ATT"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListMagic(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetMagic(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 法强
	/// </summary>
	public int GetMagic(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Magic"].AsInt;
		return  value;
	}
	/// <summary>
	/// 法强 :From Index
	/// </summary>
	public int GetMagic(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetMagic(index.ToString());
			return  value;
		}
		value = json["Magic"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListHP(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetHP(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 血量
	/// </summary>
	public int GetHP(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["HP"].AsInt;
		return  value;
	}
	/// <summary>
	/// 血量 :From Index
	/// </summary>
	public int GetHP(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetHP(index.ToString());
			return  value;
		}
		value = json["HP"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListDEF(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetDEF(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 防御
	/// </summary>
	public int GetDEF(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["DEF"].AsInt;
		return  value;
	}
	/// <summary>
	/// 防御 :From Index
	/// </summary>
	public int GetDEF(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetDEF(index.ToString());
			return  value;
		}
		value = json["DEF"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListMoveSpeed(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetMoveSpeed(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 移速
	/// </summary>
	public int GetMoveSpeed(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["MoveSpeed"].AsInt;
		return  value;
	}
	/// <summary>
	/// 移速 :From Index
	/// </summary>
	public int GetMoveSpeed(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetMoveSpeed(index.ToString());
			return  value;
		}
		value = json["MoveSpeed"].AsInt;
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
	/// 掉落
	/// </summary>
	public string GetDrop(string key){
		JSONNode json = GetJSON (key);
		string  value;
		value = json["Drop"].ToString();
		return  value;
	}
	/// <summary>
	/// 掉落 :From Index
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