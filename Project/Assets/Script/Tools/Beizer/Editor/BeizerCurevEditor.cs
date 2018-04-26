using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;
public class BeizerCurevEditor {
	private static GUIStyle sceneGUIStyle = null;
	private static GUIStyle sceneGUIStyleToolLabel = null;
	private static GUIStyle SceneGUIStyle{
		get{
			if( sceneGUIStyle == null )
			{	
				sceneGUIStyle = new GUIStyle( EditorStyles.miniTextField );
				sceneGUIStyle.alignment = TextAnchor.MiddleCenter;
			}
			return sceneGUIStyle;
		}
	}
	private static GUIStyle SceneGUIStyleToolLabel
	{
		get{
			if( sceneGUIStyleToolLabel == null )
			{	
				sceneGUIStyleToolLabel = new GUIStyle( EditorStyles.textField );
				sceneGUIStyleToolLabel.alignment = TextAnchor.MiddleCenter;
				sceneGUIStyleToolLabel.padding = new RectOffset( -8, -8, -2, 0 );
				sceneGUIStyleToolLabel.fontSize = 20;
			}
			return sceneGUIStyleToolLabel;
		}
	}
	#region Time
	static DateTime EditorDT;
	static DateTime EditordeltaTime{
		get{
			return EditorDT;
		}
	}
	static float PlayerTime{
		get{
			return (float)((DateTime.Now - EditorDT).TotalSeconds);
		}
	}
	static bool IsPlaying;
	static void Play(float scene){
		EditorDT = DateTime.Now;
		EditorDT = EditorDT.AddSeconds(-scene);
		IsPlaying = true;
	}
	static void Pause(){
		IsPlaying = false;
	}
#endregion
	static void DrawQuaternion (BeizerCurver target){
		float alllength = target.GetPathLength();
		int count = (int)(alllength/5f);
		float jiange = (float)1/count;
		for(int i = 0;i < count;i++){
			float proportion = i*jiange;
			pos = target.GetPosition(proportion);
			Handles.ArrowCap(0,pos,target.GetQuaternion(proportion),HandleUtility.GetHandleSize(pos));
		}
	}
	public static void CopyBeizer(BeizerCurver target,BeizerCurver copy){
		while (target.Points.Count != copy.Points.Count) {
			if(target.Points.Count > copy.Points.Count){
				copy.Points.Add(new BeizerPoint(target.Points[0].Point));
			}
			if(target.Points.Count < copy.Points.Count){
				copy.Points.RemoveAt(0);
			}
		}
		for(int i = 0;i < target.Points.Count;i++){
			copy.Points[i].beizerMode = target.Points[i].beizerMode;
			if(i != 0&& i!= target.Points.Count - 1)
				copy.Points[i].Point = (target.Points[i].Point - target.Points[i-1].Point)+copy.Points[i-1].Point;
			copy.Points[i].BackControlPoint = (target.Points[i].BackControlPoint - target.Points[i].Point)+copy.Points[i].Point;
			copy.Points[i].NextControlPoint = (target.Points[i].NextControlPoint - target.Points[i].Point)+copy.Points[i].Point;
		}
	} 
	static Vector3 pos;
//	public static void ShowSceneBeizerCurverLook(CurveMotionGroup cmg){
//		Handles.DrawLine (cmg.GetPosition(cmg.PlayerCurve.TestProportion*cmg.PlayerCurve.TimeLength), cmg.GetLookPosition(cmg.PlayerCurve.TestProportion*cmg.PlayerCurve.TimeLength));
//	}

	public static float GetClosestPointParamToRay(BeizerCurver target, Ray ray, int iterations, float start = 0, float end = 1, float step = .01f)
	{	
		return GetClosestPointParamIntern( (splinePos) => Vector3.Cross( ray.direction, splinePos - ray.origin ).sqrMagnitude, iterations, start, end, step,target );
	}
	
	private static float GetClosestPointParamIntern( DistanceFunction distFnc, int iterations, float start, float end, float step,BeizerCurver target )
	{
		iterations = Mathf.Clamp( iterations, 0, 5 );
		
		float minParam = GetClosestPointParamOnSegmentIntern( distFnc, start, end, step ,target);
		
		for( int i = 0; i < iterations; i++ )
		{
			float searchOffset = Mathf.Pow( 10f, -(i+2f) );
			
			start = Mathf.Clamp01( minParam-searchOffset );
			end = Mathf.Clamp01( minParam+searchOffset );
			step = searchOffset * .1f;
			
			minParam = GetClosestPointParamOnSegmentIntern( distFnc, start, end, step,target);
		}
		
		return minParam;
	}
	private static float GetClosestPointParamOnSegmentIntern( DistanceFunction distFnc, float start, float end, float step,BeizerCurver target )
	{
		float minDistance = Mathf.Infinity;
		float minParam = 0f;
		
		for( float param = start; param <= end; param += step )
		{
			float distance = distFnc( target.GetPosition( param ) );
			
			if( minDistance > distance )
			{
				minDistance = distance;
				minParam = param;
			}
		}
		
		return minParam;
	}
	
	private delegate float DistanceFunction( Vector3 splinePos );

	public static bool ShowSceneBeizerCurver(BeizerCurver target,Transform model,bool caneditor = true){
		bool change = false;
		if (caneditor ) {
			if(Event.current.alt){	
				CheckBeizer (target);
				for (int i = 0; i < target.Points.Count; i++) {
					Vector3 lastpos = target.Points [i].Point;
					target.Points [i].Point = Handles.PositionHandle (target.Points [i].Point, Quaternion.identity);
					if (GUI.changed && i != 0 && i != target.Points.Count - 1) {
						target.Points [i].BackControlPoint += target.Points [i].Point - lastpos;
						target.Points [i].NextControlPoint += target.Points [i].Point - lastpos;
					}
					if (i > 0)
						target.Points [i].m_BackControlPoint = Handles.PositionHandle (target.Points [i].m_BackControlPoint, Quaternion.identity);
					if (i < target.Points.Count - 1) {
						if (target.Points [i].beizerMode == BeizerMode.Free || i == 0) {
							target.Points [i].m_NextControlPoint = Handles.PositionHandle (target.Points [i].m_NextControlPoint, Quaternion.identity);
						}
					}
					if(target.Points[i].beizerMode == BeizerMode.Beizer){
						if(i > 0)
							target.Points[i].m_NextControlPoint = (target.Points[i].m_BackControlPoint - target.Points[i].Point).normalized*Vector3.Distance(target.Points[i].m_BackControlPoint,target.Points[i].Point)*-1+target.Points[i].Point;
					}
					Handles.color = new Color (0, 0, 0);
					Handles.Label (target.Points [i].Point + Vector3.down * (0.2f) + Vector3.right * 0.1f, "N:" + i);
				}

				for (int i = 0; i < target.Points.Count; i++) {
					if (i > 0) {
						Handles.color = Color.blue;
						Handles.SphereCap (i, target.Points [i].BackControlPoint, Quaternion.identity, HandleUtility.GetHandleSize (target.Points [i].BackControlPoint) * 0.1f);
						Handles.color = new Color (0, 0, 0.5f);
						Handles.DrawLine (target.Points [i].Point, target.Points [i].BackControlPoint);
					}
					if (i < target.Points.Count - 1) {
						Handles.color = new Color (0, 0, 0.5f);
						Handles.DrawLine (target.Points [i].Point, target.Points [i].NextControlPoint);
						Handles.color = Color.blue;
						Handles.SphereCap (i, target.Points [i].NextControlPoint, Quaternion.identity, HandleUtility.GetHandleSize (target.Points [i].NextControlPoint) * 0.1f);
					}
				}
			}
			if (Event.current.control) {
				SceneView.RepaintAll ();
				Ray mouseRay = Camera.current.ScreenPointToRay (new Vector2 (Event.current.mousePosition.x, Screen.height - Event.current.mousePosition.y - 32f));
				float splineParam =0; 
				splineParam  = GetClosestPointParamToRay (target, mouseRay, 3);
				target.TestProportion = splineParam;
				bool dele;
				float offest = 0.01f;
				int index = target.GetPointIndexWithProporiton(splineParam);
				if(index <target.Points.Count-1 && Mathf.Abs(splineParam - target.Points[index].Proportion) <(target.Points[index+1].Proportion - target.Points[index].Proportion)*offest){
					dele = true;
					index ++;
				}
				else if(index >0 && Mathf.Abs(splineParam - target.Points[index-1].Proportion) <(target.Points[index].Proportion - target.Points[index-1].Proportion)*offest ){
					dele = true;
				}
				else
					dele = false;
				Vector3 pos;
				if(dele){
					Handles.color = Color.red;
					pos = target.Points[index].Point;
				}
				else{
					Handles.color = Color.green;
					pos = target.GetProportion (splineParam);
				}
				Handles.SphereCap (5,pos , Quaternion.identity,HandleUtility.GetHandleSize (pos)*0.3f);
				if(Event.current.type == EventType.MouseDown){
					if(Event.current.button == 1){
						change = true;
						if(Event.current.alt){
							switch(target.Points[index].beizerMode){
							case BeizerMode.Beizer: target.Points[index].beizerMode = BeizerMode.Free;
								break;
							case BeizerMode.Free: target.Points[index].beizerMode = BeizerMode.Linear;
								break;
							case BeizerMode.Linear: target.Points[index].beizerMode = BeizerMode.Beizer;
								break;
							}
						}
						else{
							if(dele)
								target.Dele(index);
							else
								target.Insert(index,target.AllProportionAsPointProportion(splineParam));
						}
					}
				}
			} else {
				Handles.color = Color.red;
				if (model != null) {
					if (model.name == "LookAt" || (model.name != "LookAt" && !Application.isPlaying)) {
						try {
							model.transform.position = target.GetProportion (target.TestProportion);
						} catch {
						}
					}
				}
				if (model == null || model.name == "LookAt")
					Handles.SphereCap (10, target.GetProportion (target.TestProportion), Quaternion.identity,HandleUtility.GetHandleSize (target.GetProportion (target.TestProportion))*0.2f);
			}
		}
		Handles.color = Color.yellow;
		Vector3[] allpoints = target.GetAllPoints ().ToArray ();
		Handles.DrawAAPolyLine (allpoints);
		target.GetPathLength ();
//		bool holdcontrol;
		for (int i = 0; i < target.Points.Count; i++) {
			Handles.color = Color.green;
			Handles.SphereCap (i, target.Points [i].Point, Quaternion.identity, HandleUtility.GetHandleSize (target.Points [i].Point) * 0.2f);
		}
		if (Event.current.shift) {
			float alllengt = target.PathLength;
			float onepath = HandleUtility.GetHandleSize (target.GetProportion (0));
			float onep = onepath / alllengt;
			int count = (int)(alllengt / onepath);
			Handles.color = Color.white;
			for (int i =0; i < count; i++) {
				Handles.ArrowCap (0, target.GetPosition (i * onep), target.GetTangentQ (i * onep), HandleUtility.GetHandleSize (target.GetPosition (i * onep))*1f);
			}
		}
		if (!change && GUI.changed)
			change = true;
		return change;
	}
//	static string[] beizermodel = {"Beizer","Linear","Free"};
	static float AllTime;
	static float NowTime;
	static float MarkTime;
	static float LastProportion;
	static bool ShowInfo;
	public static void ShowBeizerCurver(BeizerCurver target,bool showtime,bool editor =false){
		if (target.TimeLength == 0)
			target.TimeLength = 1;
		AllTime = target.TimeLength;
		if(NowTime == AllTime){
			Pause();
		}
		EditorGUILayout.BeginVertical ();
		CheckBeizer (target);
		if (showtime) {
			EditorGUILayout.BeginHorizontal ();
			if (!IsPlaying) {
				if (GUILayout.Button ("Play", GUILayout.MaxWidth (40))) {
					if (NowTime == AllTime)
						NowTime = 0;
					Play (NowTime);
				}

			} else {
				NowTime = Mathf.Clamp (PlayerTime, 0, AllTime);
				SceneView.RepaintAll ();
			}
			if (IsPlaying && GUILayout.Button ("Stop", GUILayout.MaxWidth (40)))
				Pause ();
			if(target.TimeLength == 0)
				GUI.backgroundColor = Color.red;
			EditorGUILayout.LabelField ("总时间", GUILayout.MaxWidth (40));
			target.TimeLength = Mathf.Max(0,EditorGUILayout.FloatField ("", target.TimeLength, GUILayout.MaxWidth (40)));
			GUI.backgroundColor = Color.white;
			EditorGUILayout.LabelField ("当前时间", GUILayout.MaxWidth (50));
			NowTime = EditorGUILayout.FloatField ("", NowTime, GUILayout.MaxWidth (40));
			EditorGUILayout.LabelField ("标记时间", GUILayout.MaxWidth (50));
			MarkTime = EditorGUILayout.FloatField ("", MarkTime, GUILayout.MaxWidth (40));
			EditorGUILayout.EndHorizontal ();
			target.TestProportion =  EditorGUILayout.Slider (NowTime / AllTime, 0, 1);
			ShowInfo = EditorGUILayout.Foldout (ShowInfo,"Info");
			if (ShowInfo) {
				EditorGUILayout.LabelField ("PathLength" + target.PathLength);
				EditorGUILayout.LabelField ("Speed" + target.PathLength / AllTime);
			}
			if(editor){
				for(int i = 0;i < target.Points.Count;i++){
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("ID:"+i+"Point",GUILayout.Width(50));
					target.Points[i].Point = EditorGUILayout.Vector3Field("",target.Points[i].Point,GUILayout.Width(200));
					EditorGUILayout.LabelField("Back",GUILayout.Width(40));
					target.Points[i].BackControlPoint = EditorGUILayout.Vector3Field("",target.Points[i].BackControlPoint,GUILayout.Width(200));
					EditorGUILayout.LabelField("Next",GUILayout.Width(40));
					target.Points[i].NextControlPoint = EditorGUILayout.Vector3Field("",target.Points[i].NextControlPoint,GUILayout.Width(200));
					EditorGUILayout.EndHorizontal();
				}
			}
		}
		if (AllTime == 0)
			AllTime = 1;
		NowTime = AllTime*target.TestProportion ;
		target.GetPathLength ();
//		for (int i = 0; i < target.Points.Count; i++) {
//			EditorGUILayout.BeginVertical("box");
//
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.LabelField("位置:"+i,GUILayout.Width(60));
//			target.Points[i].Point = EditorGUILayout.Vector3Field("",target.Points[i].Point,GUILayout.Width(150));
//			target.Points[i].beizerMode = (BeizerMode)EditorGUILayout.Popup((int)target.Points[i].beizerMode,beizermodel);
//			EditorGUILayout.Space();
//
//			EditorGUILayout.EndHorizontal();
//
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.LabelField("节点Back:",GUILayout.Width(60));
//			target.Points[i].m_BackControlPoint = EditorGUILayout.Vector3Field("",target.Points[i].m_BackControlPoint,GUILayout.Width(150));
//			EditorGUILayout.LabelField("节点Next:",GUILayout.Width(60));
//			target.Points[i].m_NextControlPoint = EditorGUILayout.Vector3Field("",target.Points[i].m_NextControlPoint,GUILayout.Width(150));
//			EditorGUILayout.Space();
//			EditorGUILayout.EndHorizontal();
//
//			EditorGUILayout.BeginHorizontal();
//			EditorGUILayout.Space();
//			if(GUILayout.Button("+",GUILayout.Width(40)))
//				target.Insert(i);
//			if(GUILayout.Button("-",GUILayout.Width(40))){
//				target.Dele(i);
//				return ;
//			}
//			EditorGUILayout.EndHorizontal();
//
//			EditorGUILayout.EndVertical();
//
//		}
		EditorGUILayout.EndHorizontal ();
		if(GUI.changed)
			SceneView.RepaintAll ();
	}
	public static void CheckBeizer(BeizerCurver target){
		if (target.Points == null  && target.Points.Count == 0) {
			target.Points = new System.Collections.Generic.List<BeizerPoint>();
			target.Points.Add(new BeizerPoint(new Vector3(0,0,0)));
			target.Points.Add(new BeizerPoint(new Vector3(0,0,1)));
		}
	}

}