using UnityEngine;
using System.Collections;
using UnityEditor;
[CustomEditor(typeof(BeizerCamera))]
public class BeizerCameraInspector : Editor {
	BeizerCamera Target;
	public void OnEnable ()
	{
		Target = target as BeizerCamera;
	}
	public void OnSceneGUI(){
		BeizerCurevEditor.ShowSceneBeizerCurver (Target.CameraPath,null);
		BeizerCurevEditor.ShowSceneBeizerCurver (Target.TargetPath,null);
//		BeizerCurevEditor.
	}
	public override void OnInspectorGUI ()
	{
		BeizerCurevEditor.ShowBeizerCurver (Target.CameraPath,true,true);
		BeizerCurevEditor.ShowBeizerCurver (Target.TargetPath,true,true);
	}
}
