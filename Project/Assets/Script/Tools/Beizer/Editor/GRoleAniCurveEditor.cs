//using UnityEngine;
//using System.Collections;
//
//using UnityEditor;
//[CustomEditor(typeof(GRoleAniCurve))]
//public class GRoleAniCurveEditor : Editor {
//	GRoleAniCurve Target;
//	void OnEnable(){
//		if(Target == null)
//			Target = target as GRoleAniCurve;
//	}
//	public void OnSceneGUI () {
//		if (Target.RoleCurve != null&& Target.RoleCurve.Count >selectRoleCure) {
//			BeizerCurevEditor.ShowSceneBeizerCurver(Target.RoleCurve[selectRoleCure].PlayerCurve,Target.TestModel);
//			if(!Target.RoleCurve [selectRoleCure].LookForwn)
//				BeizerCurevEditor.ShowSceneBeizerCurver(Target.RoleCurve[selectRoleCure].TargetLookCurve,Target.TargetLook);
//			else
//				Target.TargetLook.position = Target.RoleCurve[selectRoleCure].GetLookPosition(Target.RoleCurve[selectRoleCure].PlayerCurve.TestProportion * Target.RoleCurve[selectRoleCure].PlayerCurve.TimeLength);
//			BeizerCurevEditor.ShowSceneBeizerCurverLook(Target.RoleCurve[selectRoleCure]);
//			if(Target.TestModel != null)
//				Target.TestModel.LookAt(Target.TargetLook);
//
//		}
//		Repaint ();
//	}
//	int selectRoleCure = 0;
//	public override void OnInspectorGUI(){
//		if (Target.RoleCurve == null)
//			Target.RoleCurve = new System.Collections.Generic.List<CurveMotionGroup> ();
//		if (Target.RoleCurve.Count == 0) {
//			Target.RoleCurve.Add(new CurveMotionGroup());
//		}
//		string[] tags = new string[Target.RoleCurve.Count];
//		for (int i =0; i <tags.Length; i++) {
//			tags[i] = Target.RoleCurve[i].Tag;
//		}
//		Target.TestModel = EditorGUILayout.ObjectField(Target.TestModel,typeof(Transform)) as Transform;
//		EditorGUILayout.BeginHorizontal ();
//
//		selectRoleCure = EditorGUILayout.Popup (selectRoleCure, tags,GUILayout.MaxWidth(100));
//		EditorGUILayout.LabelField ("Tag", GUILayout.MaxWidth (40));
//		Target.RoleCurve [selectRoleCure].Tag = EditorGUILayout.TextField ("", Target.RoleCurve [selectRoleCure].Tag, GUILayout.MaxWidth (60));
//
//		if (!Target.RoleCurve [selectRoleCure].LookForwn && GUILayout.Button ("注视前方",GUILayout.MaxWidth (100)))
//			Target.RoleCurve [selectRoleCure].LookForwn = true;
//		if (Target.RoleCurve [selectRoleCure].LookForwn && GUILayout.Button ("注视目标路径",GUILayout.MaxWidth (100)))
//			Target.RoleCurve [selectRoleCure].LookForwn = false;
//
//		EditorGUILayout.Space ();
//		if (GUILayout.Button ("+",GUILayout.MaxWidth (60))) {
//			Target.RoleCurve.Add (new CurveMotionGroup ());
//			selectRoleCure = Target.RoleCurve.Count -1;
//		}
//		if (Target.RoleCurve.Count > 1 && GUILayout.Button ("-",GUILayout.MaxWidth (60))) {
//			Target.RoleCurve.RemoveAt (selectRoleCure);
//			selectRoleCure --;
//			return;
//		}
//
//		EditorGUILayout.EndHorizontal ();
//
//		if (Target.TargetLook == null) {
//			Target.TargetLook = new GameObject ("LookAt").transform;
//			Target.TargetLook.transform.parent = Target.transform;
//		}
//		Target.RoleCurve [selectRoleCure].TargetLookCurve.TimeLength = Target.RoleCurve [selectRoleCure].PlayerCurve.TimeLength;
//		BeizerCurevEditor.ShowBeizerCurver(Target.RoleCurve[selectRoleCure].PlayerCurve,true);
//		if(!Target.RoleCurve [selectRoleCure].LookForwn)
//			BeizerCurevEditor.ShowBeizerCurver(Target.RoleCurve[selectRoleCure].TargetLookCurve,false);
//		Repaint ();
//	}
//}
