//#define PC版调试
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;
using System.Text;
using System.IO;   
public static class JsonRead {

//	public static JSONNode ReadFormIO(string String){
//		FileInfo t; 
//		t = new FileInfo("C:/GameData/"+ String+".txt");   
//		StreamReader r = t.OpenText(); 
//		string str;
//		str = r.ReadToEnd ();
//		JSONNode json;
//		json = Parse (str.ToString ());
//		return json;
//	}
	public static JSONNode Read(string String,string data = "Data/"){
		TextAsset Text;
		JSONNode jsondata = new JSONClass();

			Text = Resources.Load (data + String) as TextAsset;
			try {
				jsondata = JSONNode.Parse (Text.text);
			} catch {
			}

		return jsondata;
	}
	public static JSONNode Parse( string String){	
		JSONNode jsondata = new JSON.JSONClass();
		jsondata = JSON.JSONNode.Parse(String);
		return jsondata;
	}
	public static Rect AsRect(JSONNode json){
		Rect r = new Rect ();
		r.x = json[0].AsFloat;
		r.y = json[1].AsFloat;
		r.width = json[2].AsFloat;
		r.height = json[3].AsFloat;
		return r;
	}
	public static Vector2 AsVector2(JSONNode json){
		Vector2 v2 = new Vector2();
		v2.x = json [0].AsFloat;
		v2.y = json [1].AsFloat;
		return v2;
	}
	public static Vector3 AsVector3(JSONNode json){
		Vector3 v3 = new Vector3 ();
		v3.x = json [0].AsFloat;
		v3.y = json [1].AsFloat;
		v3.z = json [2].AsFloat;
		return v3;
	}
	public static float[] AsFloatS(JSONNode json){
		float[] fs;
		int count;
		count = json.Count;
		fs = new float[count];
		for(int i = 0;i < count;i++){
			fs[i] = json[i].AsFloat;
		}
		return fs;
	}
	public static string[] AsStrings(JSONNode json){
		string[] fs;
		int count;
		count = json.Count;
		fs = new string[count];
		for(int i = 0;i < count;i++){
			fs[i] = json[i].ToString();
		}
		return fs;
	}
	public static int[] AsIntS(JSONNode json){
		int[] fs;
		int count;
		count = json.Count;
		fs = new int[count];
		for(int i = 0;i < count;i++){
			fs[i] = json[i].AsInt;
		}
		return fs;
	}
	public static JSONNode FindJSONNodeWithType(JSONNode json,string type,string like){
		JSONClass newjson = new JSONClass ();
		for (int i = 0; i < json.Count; i++) {
			if(json[i][type].ToString() == like){
				newjson.Add(json[i]);
			}
		}
		return (JSONNode)newjson;
	}


}
public enum jsontype{
	e_String,
	e_Int,
	e_Float,
	e_JsonData,
	e_JsonList,
}
//"":"";
public class JsonData{
	public string key;
	public string value;
	public int type;
	public static JsonData Init(string _key,string _value){
		JsonData jd = new JsonData();
		jd.key = _key;
		jd.value = _value;
		return jd;
	}
	public static JsonData Init(string key,int value){
		return Init(key,value.ToString());
	}
	public static JsonData Init(string key,float value){
		return Init(key,value.ToString());
	}
	public static JsonData Init(string key,JsonData value){
		string str;
			str = "["+value.GetString()+"]";
		return Init(key,str);
	}
	public static JsonData Init(ArrJsonData arr){
		return Init(arr.key,arr.GetString());
	}
	public static JsonData Init(ListJsonData value){
		string str;
		JsonData jd = new JsonData();
			str = value.GetString();
			jd = Init("",str);
			jd.type = (int)jsontype.e_JsonList;
		return jd;
	}
	/// <summary>
	/// "key":value
	/// </summary>
	/// <returns>The string.</returns>
	public string GetString(){
		string str = "";	
		if(type == (int)jsontype.e_JsonList)
				str += value;
			else{
				str += "\""+key + "\":" +value ;
			}
			return str;
	}
	///
	public string AsString(){
		return "{"+GetString()+"}";
	}
}

//{},{}
public class ListJsonData{
	public List<JsonData> list = new List<JsonData>();
	public bool m_NextLine;
	public void Clear(){
		list.Clear();
	}
    public JsonData GetData(string key)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (list[i].key == key)
                return list[i];
        }
        return null;
    }
	public JsonData InitJsonData(string _key,string _value){
		JsonData jd = new JsonData();
		jd.key = _key;
		jd.value = _value;
		return jd;
	}

	public void Add(string key,string value){
		if (value.Length >= 2 && value [0] == '{' && value [value.Length - 1] == '}') {
			Add (InitJsonData (key, value));
		} else {
			Add (InitJsonData (key, "\"" + value + "\""));
		}
	}
	public void Add(string key,int value){
		Add(InitJsonData(key,value.ToString()));
	}
	public void Add(string key,Vector2 value){
		StringBuilder str = new StringBuilder ();
		str.Append ("[");
		str.Append (value.x.ToString ()+",");
		str.Append (value.y.ToString ());
		str.Append ("]");
		Add(InitJsonData(key,str.ToString()));
	}
	public void Add(string key,Vector3 value){
		StringBuilder str = new StringBuilder ();
		str.Append ("[");
		str.Append (value.x.ToString ()+",");
		str.Append (value.y.ToString ()+",");
		str.Append (value.z.ToString ());
		str.Append ("]");
		Add(InitJsonData(key,str.ToString()));
	}
	public void Add(string key,int[] value){
		StringBuilder str = new StringBuilder ();
		str.Append ("[");
		for (int i = 0; i < value.Length; i++) {
			str.Append (value [i].ToString () + (i<value.Length-1?",":""));
		}
		str.Append ("]");
		Add(InitJsonData(key,str.ToString()));
	}
	public void Add(string key,float value){
		Add(InitJsonData(key,value.ToString()));
	}
	public void Add(ArrJsonData arr){
		Add(InitJsonData(arr.key,arr.GetString()));
	}
	public void Add(string key,JsonData value){
		string str;
		str = "["+value.GetString()+"]";
		Add(InitJsonData(key,str));
	}
	public void Add(string key,ListJsonData value){
		string str;
		str = value.GetString();
		Add(InitJsonData(key,str));
	}

	public JsonData getJsonData(string key){
		for(int i = 0;i < list.Count;i++){
			if (list [i].key == key)
				return list [i];
		}
		return null;
	}
	
	
	public static ListJsonData Init(){
		ListJsonData ljd = new ListJsonData();
		return ljd;
	}
	public void Add(JsonData jd){
		list.Add(jd);
	}
	/// <summary>
	/// {"key":value,"key2",value}
	/// </summary>
	/// <returns>The string.</returns>
	public string GetString(){
		string str = "";
		if(m_NextLine)
			str += "{"+"\n\r";
		else
			str += "{";
		for(var i = 0;i <list.Count;i++){
			str += list[i].GetString();
			if(i < list.Count - 1){
				str += ",";

			}
			if (m_NextLine)
				str += "\n";
		}
		if(m_NextLine)
			str += "}"+"\n";
		else
			str += "}";

		return str;
	}
	public JSONNode AsJSONNode{
		get{
			return JsonRead.Parse(GetString());
		}
	}
}
//"":[]
public class ArrJsonData{

	public string key;
	public string value ="";
	public List<JsonData> list = new List<JsonData>();
	public bool NewLine;
	public void Clear(){
		list.Clear();
		value = "";
	}
	public static ArrJsonData Init(string _key){
		ArrJsonData ajd = new ArrJsonData();
		ajd.key = _key;
		ajd.value = "";
		return ajd;
	}
	public void Add(string str){
		value += (value == ""?"":",")+str;
	}
	public void Add(int[] vints){
		System.Text.StringBuilder builder = new System.Text.StringBuilder ();
		builder.Append ("[");
		for (int i = 0; i < vints.Length; i++) {
			builder.Append ( vints [i]  + (i == vints.Length - 1 ? "" : ","));
		}
		builder.Append ("]");
		Add (builder.ToString());
	}
//	public void Add(float[] vints){
//		System.Text.StringBuilder builder = new System.Text.StringBuilder ();
//		for (int i = 0; i < vints.Length; i++) {
//			builder.Append ("[" + vints [i] + "]" + (i == vints.Length - 1 ? "" : ","));
//		}
//		Add (builder.ToString());
//	}
//	public void Add(string[] vints){
//		System.Text.StringBuilder builder = new System.Text.StringBuilder ();
//		for (int i = 0; i < vints.Length; i++) {
//			builder.Append ("[" + vints [i] + "]" + (i == vints.Length - 1 ? "" : ","));
//		}
//		Add (builder.ToString());
//	}
	public void Add(Object[] vints){
		System.Text.StringBuilder builder = new System.Text.StringBuilder ();
		for (int i = 0; i < vints.Length; i++) {
			builder.Append ("[" + vints [i].ToString() + "]" + (i == vints.Length - 1 ? "" : ","));
		}
		Add (builder.ToString());
	}
	public void Add(int vint){
		Add(vint.ToString());
	}
	public void Add(float vf){
		Add(vf.ToString());
	}
	public void Add(Vector3 VE3){
		Add(VE3.x);
		Add(VE3.y);
		Add(VE3.z);
	}
	public void Add(Rect rect){
		Add(rect.x);
		Add(rect.y);
		Add(rect.width);
		Add(rect.height);
	}
	public void Add(ArrJsonData arr){
		Add(arr.GetString());
	}
	public void Add(ListJsonData list){
		Add(JsonData.Init(list));
	}
	public void Add(Vector2 v2){
		Add(v2.x);
		Add(v2.y);
	}
	public void Add(JsonData _value){
		list.Add(_value);
	}
	/// <summary>
	/// ["value","value"]
	/// </summary>
	/// <returns>The string.</returns>
	public string GetString(){
		string str;
			if(value == ""){
				str = "[";
				if(NewLine)	
					str+="\n";
				for(var i = 0;i < list.Count;i++){
					str += list[i].GetString();
					if(i < list.Count - 1){
						if(NewLine)	
							str += ","+"\n";
						else
							str+=",";
					}
				}
			}
			else{
				str = "[";	
				str += value;
			}
		if(NewLine)	
			str+="\n";
		str += "]";
		return str;
	}
	public JSONNode AsJsonNode(){
		JSONNode json;
		json = JSON.JSONNode.Parse(GetString());
		return json;
	}
	public string ToJSONString(){
		return AsJsonNode().ToString();
	}
	public string AsString(){
		JsonData jd;
			jd  = JsonData.Init(this);
			return jd.AsString();
	}
}
/// <summary>
/// JsonData	
/// 	JsonData 是基本JSON格式 
/// 	通过 Init();
/// 	可以 Key:value;  		例如 "Key":"aa",
/// 	可以 Key:ArrJsonData		例如 "key":["value"],
/// 	可以 Key:ListJsonData 	例如	"key":{xxx}; 
/// ListJsonData
/// 	大括号{}
/// 	可以Add("key",value) 存入 JsonData 结果为{"key":value};
/// 	可以Add(jsonarr) 存入ArrJsonData 结果为{"key":[value,value]}
/// ArrJsonData
/// 	[]数组形式
/// 	可以Add(ListJsonData) 	结果为"key":[{},{}],
/// 	可以Add(int string float vector2 vector3 rect```) 但必须先Init(“key”)	结果为"key":[int,string,float,vector2,vector3,rect````],
/// ----------------------------------------例子
//JsonData JsonData1 = new JsonData();
//ArrJsonData jsonarr = new ArrJsonData();
//ListJsonData list;
//
//list = ListJsonData.Init();
//list.Add("id",1);
//list.Add("name","abc");
//jsonarr = ArrJsonData.Init("pos");
//jsonarr.Add(new Vector3(0,19,2));
//list.Add(jsonarr);
//JsonData1 = JsonData.Init(list);
//jsonarr.Clear();
//jsonarr = ArrJsonData.Init("players");
//jsonarr.Add(JsonData1);
//
//list.Clear();
//list.Add("id","2");
//list.Add("name","ddd");
//JsonData1 = JsonData.Init(list);
//jsonarr.Add(JsonData1);
//
//JSONNode json;
//json = JsonRead.Parse(jsonarr.GetString());
//PLAYER:[ {"id":1, "name":abc, "pos":[ 0, 19, 2 ]}, {"id":2, "name":ddd} ]
/// </summary>

