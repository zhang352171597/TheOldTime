//using UnityEngine;
//using System;
//using System.Collections;
//using System.Reflection;
//using System.Collections.Generic;
//
//[System.Serializable]
//public class CurveMotionGroup:StateMachineBase{
//	public CurveMotionGroup(){
//		PlayerCurve = new BeizerCurver ();
//		TargetLookCurve = new BeizerCurver ();
//	}
//	public BeizerCurver PlayerCurve;
//	public BeizerCurver TargetLookCurve;
//	public bool LookForwn;
//	public string Tag;
//	public void Update(float time){
//
//	}
//	public Vector3 GetPosition(float time){
//		return PlayerCurve.GetPosition(time / PlayerCurve.TimeLength);
//	}
//	public Quaternion GetQuaternion(float time){
//		return PlayerCurve.GetQuaternion(time / PlayerCurve.TimeLength);
//	}
//	public Vector3 GetLookPosition(float time){
//		if (LookForwn) {
//			float proportion = time / PlayerCurve.TimeLength;
//			return PlayerCurve.GetPosition (0.01f+proportion);
//		}
//		else
//			return TargetLookCurve.GetPosition (time / PlayerCurve.TimeLength);
//	}
//
//	public void DrawGizmos(){
//		PlayerCurve.OnDrawGizmos ();
//	}
//}
//public class GRoleAniCurve : MonoBehaviour {
//	public List<CurveMotionGroup> RoleCurve = new List<CurveMotionGroup>();
//	// Use this for initialization
//	// Update is called once per frame
//	public float deleyTime;
//	public Transform TestModel;
//	public Transform TargetLook;
//	private float savetime;
//	void Update () {
//		TestModel.transform.position = RoleCurve [0].GetPosition (Time.time-savetime);		 
//		TargetLook.transform.position = RoleCurve [0].GetLookPosition (Time.time-savetime);		 
//		TestModel.transform.LookAt (TargetLook);
//		if(Input.GetKeyDown("1"))
//		   savetime = Time.time;
//	}
//
//	#region Gizmo
//	void OnDrawGizmos () {
//		if (RoleCurve != null) {
//			for(int i = 0;i <RoleCurve.Count;i++){
//				RoleCurve[i].DrawGizmos();
//			}
//		}
//	}
//	#endregion
//}
