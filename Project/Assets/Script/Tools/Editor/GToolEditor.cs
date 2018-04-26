using UnityEngine;
using System.Collections;
using UnityEditor;
public class GToolEditor : EditorWindow {
	[MenuItem("GTool/TimeScale ")]
	public static void Init(){
		EditorWindow.GetWindow<GToolEditor> ();
	}


	public void OnGUI(){
		EditorGUILayout.LabelField ("Time Scale: now:"+Time.timeScale);
		EditorGUILayout.BeginHorizontal ("box");
		for(int i = 0;i<10;i++){
			if(GUILayout.Button(i.ToString()))
				Time.timeScale = i;
		}
		EditorGUILayout.EndHorizontal ();
		EditorGUILayout.BeginHorizontal ("box");
		for (float i = 0; i < 1; i += 0.1f) {
			if(GUILayout.Button(i.ToString("f1")))
				Time.timeScale = i;
		}
		EditorGUILayout.EndHorizontal ();
	}
}
