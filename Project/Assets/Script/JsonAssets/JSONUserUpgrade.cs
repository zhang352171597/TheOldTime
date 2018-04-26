using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class JSONUserUpgrade{
	#region 单例
	private static JSONUserUpgrade instance;
	public static JSONUserUpgrade getInstance(){
	if (instance == null)
		instance = new JSONUserUpgrade ();
		return instance;
	}
	#endregion

	#region 变量
	private JSONNode json;
	private JSONNode JSONData{
		get{
			if(json == null)
				json = JsonRead.Read("JSONUserUpgrade");
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
	public int[] GetListLV(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetLV(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 等级
	/// </summary>
	public int GetLV(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["LV"].AsInt;
		return  value;
	}
	/// <summary>
	/// 等级 :From Index
	/// </summary>
	public int GetLV(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetLV(index.ToString());
			return  value;
		}
		value = json["LV"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListEXP(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetEXP(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 升级所需经验
	/// </summary>
	public int GetEXP(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["EXP"].AsInt;
		return  value;
	}
	/// <summary>
	/// 升级所需经验 :From Index
	/// </summary>
	public int GetEXP(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetEXP(index.ToString());
			return  value;
		}
		value = json["EXP"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListAD(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetAD(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 物攻
	/// </summary>
	public int GetAD(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["AD"].AsInt;
		return  value;
	}
	/// <summary>
	/// 物攻 :From Index
	/// </summary>
	public int GetAD(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetAD(index.ToString());
			return  value;
		}
		value = json["AD"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListAP(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetAP(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 魔攻
	/// </summary>
	public int GetAP(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["AP"].AsInt;
		return  value;
	}
	/// <summary>
	/// 魔攻 :From Index
	/// </summary>
	public int GetAP(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetAP(index.ToString());
			return  value;
		}
		value = json["AP"].AsInt;
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
	public int[] GetListDF(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetDF(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 物抗
	/// </summary>
	public int GetDF(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["DF"].AsInt;
		return  value;
	}
	/// <summary>
	/// 物抗 :From Index
	/// </summary>
	public int GetDF(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetDF(index.ToString());
			return  value;
		}
		value = json["DF"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListPF(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetPF(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 魔抗
	/// </summary>
	public int GetPF(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["PF"].AsInt;
		return  value;
	}
	/// <summary>
	/// 魔抗 :From Index
	/// </summary>
	public int GetPF(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetPF(index.ToString());
			return  value;
		}
		value = json["PF"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListCrit(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetCrit(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 暴击
	/// </summary>
	public int GetCrit(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Crit"].AsInt;
		return  value;
	}
	/// <summary>
	/// 暴击 :From Index
	/// </summary>
	public int GetCrit(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetCrit(index.ToString());
			return  value;
		}
		value = json["Crit"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListSpeed(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetSpeed(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 移速
	/// </summary>
	public int GetSpeed(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Speed"].AsInt;
		return  value;
	}
	/// <summary>
	/// 移速 :From Index
	/// </summary>
	public int GetSpeed(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetSpeed(index.ToString());
			return  value;
		}
		value = json["Speed"].AsInt;
		return  value;
	}
    /// <summary>
	/// GetJSONNodeList Like Target
	/// </summary>
	public int[] GetListLucky(int target){
		List<int> list = new List<int>();
		for (int i = 0; i < GetCount(); i++) {
			if( GetLucky(i) == target)
				list.Add(i);
		}
		return list.ToArray();
	}
	/// <summary>
	/// 幸运
	/// </summary>
	public int GetLucky(string key){
		JSONNode json = GetJSON (key);
		int  value;
		value = json["Lucky"].AsInt;
		return  value;
	}
	/// <summary>
	/// 幸运 :From Index
	/// </summary>
	public int GetLucky(int index){
		int value;
		JSONNode json = GetJSON (index);
		if(json == null){
			value = GetLucky(index.ToString());
			return  value;
		}
		value = json["Lucky"].AsInt;
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