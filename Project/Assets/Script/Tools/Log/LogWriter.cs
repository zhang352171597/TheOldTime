using UnityEngine;
using System.Collections;
using System.IO;

public class LogWriter<T> where T:new(){
	protected static T instance;
	public static T Instance{
		get{ 
			if (instance == null) {
				instance = new T ();
			}
			return instance;
		}
	}

	string filename = "BattleLog";
	public string savePath{
		get{
            if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
                return "";
			string mypath = Application.dataPath.Replace ("Assets", "");
			CheckFile (mypath + "/GameLog");
			return  mypath + "/GameLog/"+SystemInfo.deviceUniqueIdentifier+"-"+filename+"_";
//			return  mypath + "/GameLog/"+ NetBattle.LobbyData.Instance.userID+"-"+filename+"_";
		}
	}
	public void CheckFile(string path){
		#if UNITY_EDITOR
		if (!Directory.Exists (path))
			Directory.CreateDirectory (path);
		#endif
	}
	System.IO.FileInfo logerFileInfo;
	System.IO.StreamWriter writer;
	string path;
	public LogWriter(){
		#if UNITY_EDITOR
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
            return;
		filename = typeof(T).Name;
		int i = 0;
		while(logerFileInfo == null){
			path = savePath + i + ".txt";
			logerFileInfo = new System.IO.FileInfo (path);
			if (logerFileInfo.Exists) {
				
				logerFileInfo = null;
				i++;
			} else {
//				Debug.Log ("GameLog Path:" + path);		
			}
		}
		#endif
	}

	public void AddLog(params object[] msgs){
		#if UNITY_EDITOR
		string msg = "";
		for (int i = 0; i < msgs.Length; i++) {
			msg += msgs [i].ToString ();
		}
		AddLog (msg);
		#endif
	}

	public virtual void AddLog(string msg){
		#if UNITY_EDITOR
		if(writer == null)
			writer = logerFileInfo.CreateText ();
		writer.WriteLine (msg);
		#endif
	}
	public virtual void Close(){
		#if UNITY_EDITOR
		if (writer != null)
			writer.Close ();
		#endif
	}
}