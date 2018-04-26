//using UnityEngine;
//using System.Collections;
//
//using UnityEditor;
//[CustomEditor(typeof(BeizerLine))]
//public class BeizerLineEditor : Editor {
//	BeizerLine Target;
//	void OnEnable(){
//		if(Target == null)
//			Target = target as BeizerLine;
//	}
//	public void OnSceneGUI () {
//		BeizerCurevEditor.ShowSceneBeizerCurverLook(Target.Curver);
//		Repaint ();
//	}
//	int selectRoleCure = 0;
//	public override void OnInspectorGUI(){
//		BeizerCurevEditor.ShowBeizerCurver(Target.Curver,true);
//		Repaint ();
//	}
//}
