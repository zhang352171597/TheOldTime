using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class CheckBattleLoger : EditorWindow {
	public static string LogPath{
		get{ 
			string path = Application.dataPath.Replace ("Assets", "") + "/GameLog";
			if (!Directory.Exists (path))
				Directory.CreateDirectory (path);
			return path ;
		}
	}
	[MenuItem("GTool/日记查看文档")]
	public static void Init(){
		var window = GetWindow<CheckBattleLoger> ();
		window.FindLogers ();
	}
	public static string[] ST = new string[0];
	string[] logers;
	int soureid = 0;
	EdBattleLoger soure;
	int targetid = 0;
	EdBattleLoger target;
	Vector2 pos;
	Vector2 stpos;
	bool seting;
	float showIndex;
	public static string searchInfo = "";
	public static bool onlyShowError;
	public static string selectInfo;
	public void FindFirstError(){
		if (target == null || target.infos == null || soure == null || soure.infos == null)
			return;
		int length = target.infos.Length > soure.infos.Length ? soure.infos.Length : target.infos.Length; 
		for (int i = 0; i < length; i++) {
			if (target.infos [i] != soure.infos [i]) {
				showIndex = i * 10;
				return;
			}
		}
	}
	public void OnFocus(){
		FindLogers ();	
	}
	public void DeleLog(){
		for (int i = 0; i < logers.Length; i++) {
			string path = LogPath + "/" + logers [i];
			File.Delete (path);
		}
	}
	public void OnGUI(){
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("刷新")) {
			FindLogers ();
		}
		if (GUILayout.Button ("定位")) {
			FindFirstError ();
		}
		if (GUILayout.Button ("Dele")) {
			if(EditorUtility.DisplayDialog("警告","是否要删除所有日记","是","否"))
				DeleLog ();
		}
		if (GUILayout.Button ("设置"))
			seting = !seting;
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginVertical ();
			EditorGUILayout.BeginHorizontal ();
			onlyShowError = EditorGUILayout.Toggle ("显示错误",onlyShowError);
			searchInfo = EditorGUILayout.TextField (searchInfo);
			EditorGUILayout.EndHorizontal ();
			stpos = EditorGUILayout.BeginScrollView (stpos);
			for (int i = 0; i < ST.Length; i++) {
				string[] stsp = ST [i].Split (':');
				EditorGUILayout.BeginHorizontal ();
					for (int x = 0; x < stsp.Length; x++) {
					GUILayout.TextField (stsp[x],GUILayout.Width(200));	
					}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();

		Event myevent = Event.current;
		if(myevent.shift)
			showIndex += myevent.delta.y*2;
		if (showIndex < 0)
			showIndex = 0;
		EditorGUILayout.BeginHorizontal ();
		SelectBattle (ref soure,ref soureid);
		SelectBattle (ref target,ref targetid);
		EditorGUILayout.EndHorizontal ();
		int right;
		int tl = 0;
		int sl = 0;

		if (target != null && target.infos != null)
			tl = target.infos.Length;
		if (soure != null && soure.infos != null)
			sl = soure.infos.Length;

		int start = 0;
		int length = 10;
		int h = (int)(Screen.height);
		h = (int)(h - GUIStyle.none.lineHeight * 5);
		length = (int)((h /((GUIStyle)("box")).lineHeight)*0.5f);

		right = sl < tl ? (sl-length) * 10 : (tl-length) * 10;
		showIndex = EditorGUILayout.Slider (showIndex, 0, right);
		pos = EditorGUILayout.BeginScrollView (pos);
		EditorGUILayout.BeginHorizontal ();
			EditorGUILayout.BeginVertical ();

			start = (int)(showIndex * 0.1f);
			
			if (soure != null) 
				soure.ShowGUI (soure,start,length);
			EditorGUILayout.EndVertical ();

			EditorGUILayout.BeginVertical ();
			
			if (target != null)
				target.ShowGUI (soure,start,length);
			EditorGUILayout.EndVertical ();

		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.EndScrollView ();
	}
	void SelectBattle(ref EdBattleLoger loger,ref int index){
		if (logers == null)
			return;
		
		index = EditorGUILayout.Popup (index, logers);
		if (GUI.changed) {
			try{
			loger = new EdBattleLoger(logers[index]);
			}catch{
			}
		}
	}
	void FindLogers(){
		string[] arrloger = Directory.GetFiles (LogPath);
		List<string> listloger = new List<string> ();

		for (int i = 0; i < arrloger.Length; i++) {
			if (arrloger[i].Contains ("GameLog_")) {
				listloger.Add (Path.GetFileName(arrloger [i]));
			}
		}
		listloger.Sort (CompareTo);
		logers = listloger.ToArray ();
		if (listloger.Count > 1) {
			soureid = logers.Length - 2;
			targetid = logers.Length - 1;
			target = new EdBattleLoger (logers [targetid]);
			soure =  new EdBattleLoger (logers [soureid]);
		}
		FindFirstError ();
	}

	public static int CompareTo(string v1,string v2){
		int value1 = 0;
		int value2 = 0;
		try{
			v1 = Path.GetFileNameWithoutExtension(v1);
		string[] sp = v1.Split ('_');
		value1 = int.Parse (sp [1]);
		}catch{
			return -1;
		}
		try{
			v2 = Path.GetFileNameWithoutExtension(v2);
		string[] sp2 = v2.Split ('_');
		value2 = int.Parse (sp2 [1]);
		}catch{
			return 1;
		}
		return value1.CompareTo (value2);
	}
}

public class EdBattleLoger{
	public string[] infos;
	public string[] StackTraceInfo;
	public EdBattleLoger(string path){
		path = CheckBattleLoger.LogPath+"/" + path;
		FileInfo fileinfo = new FileInfo (path);
		if (fileinfo.Exists) {
			StreamReader reader = fileinfo.OpenText ();
			System.Collections.Generic.List<string> strs = new System.Collections.Generic.List<string> ();
			while (reader.Peek() >= 0) {
				strs.Add(reader.ReadLine ());
			}
			infos = strs.ToArray ();
			reader.Close ();
		}
		StackTraceInfo = new string[infos.Length];
		for (int i = 0; i < infos.Length; i++) {
			string st = RegexMatch (infos [i]);
			StackTraceInfo [i] = st;
			try{
				infos [i] = infos [i].Replace (st, GetFileName (st));
			}catch{
			}
		}

	}
	public string GetFileName(string source){
		string[] filenameinfo = source.Split ('|');
		try{
		return filenameinfo [0];
		}catch{
			return " ";
		}
	}
	public string RegexMatch(string source){
		string value = Regex.Match (source, @"\[(.*?)\]", RegexOptions.Singleline).ToString();
		try{
		value = value.Substring (1, value.Length - 2);
		}catch {
		}
		return value;
	}

	public void ShowGUI(EdBattleLoger target,int start,int max){
		if (infos == null)
			return;
		int count = 10;
		try{
		count = start + max;
//		GUIStyle gstyle = "RL Background";
			GUIStyle gstyle = "AssetLabel";
		Color gstylecolor = gstyle.active.textColor;

		gstyle.active.textColor = Color.white;
		for (int i = start; i < count; i++) {


			Color col = Color.white;
			bool errorinfo = false;
			if (target != null && target != this) {
				if (target.infos.Length > i) {
					errorinfo = target.infos [i] != infos [i];
					if (errorinfo) {
						col = Color.red;
					} else
						col = Color.white;
				}		
			}
			GUI.backgroundColor = col;
			EditorGUILayout.BeginHorizontal ("box");
			string labinfo = "";
			try{
				labinfo = i.ToString ().PadLeft (4, '0') + ":" + infos [i];
			}
			catch{
				
			}
			string selectinfo = "";

			string[] splitlabinfo = labinfo.Split (':');
			int startsp = 2;
			for(int x = startsp; x < splitlabinfo.Length;x++){
				selectinfo += splitlabinfo[x];
			}
			
			if (CheckBattleLoger.selectInfo == selectinfo)
				GUI.backgroundColor = Color.white;
				if (GUILayout.Button (labinfo,gstyle)){
					CheckBattleLoger.selectInfo = selectinfo;
					CheckBattleLoger.ST = StackTraceInfo[i].Split('|');
				}
			EditorGUILayout.EndHorizontal ();
		}
		GUI.backgroundColor = Color.white;
		gstyle.active.textColor = gstylecolor;
		}catch{
		}
	}
}