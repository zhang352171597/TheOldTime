using UnityEngine;
using System.Collections;

using UnityEditor;
[CustomEditor(typeof(BeizerLine))]
public class BeizerLineEditor : Editor {
	BeizerLine Target;
	void OnEnable(){
		if(Target == null)
			Target = target as BeizerLine;
	}
	public void OnSceneGUI () {
		BeizerCurevEditor.ShowSceneBeizerCurver(Target.Curver,null);
		Repaint ();
	}
//	int selectRoleCure = 0;
	public override void OnInspectorGUI(){
		BeizerCurevEditor.ShowBeizerCurver(Target.Curver,true,true);
		EditorGUILayout.LabelField (Target.Curver.TestProportion.ToString());
		Repaint ();
		Target.autoPlay = EditorGUILayout.Toggle ("编辑器下测试",Target.autoPlay);
		if (Target.autoPlay)
			Target.UpdatePos (Target.Curver.TestProportion);
	}
}
