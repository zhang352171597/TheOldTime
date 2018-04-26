using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum BeizerMode{
	Beizer,
	Linear,
	Free,
}
[System.Serializable]
public class BeizerPoint{
	public BeizerPoint(Vector3 point){
		Point = point;
		m_BackControlPoint = Point+Vector3.down;
		m_NextControlPoint = Point+Vector3.up;
	}
	public Vector3 Point;
	public Vector3 m_BackControlPoint;
	/// <summary>
	/// 下一个控制点
	/// </summary>
	public	Vector3 BackControlPoint{
		get
		{
			return m_BackControlPoint;
		}
		set{
			m_BackControlPoint = value;
		}
	}
	public Vector3 m_NextControlPoint;
	/// <summary>
	/// 上一个控制点
	/// </summary>
	public Vector3 NextControlPoint{
		get
		{
			return m_NextControlPoint;
		}
		set{
			m_NextControlPoint = value;
		}
	}
	public BeizerMode beizerMode;
	public float Length;
	public float Proportion = 1;
	public Vector3[] CalculateBeizer(BeizerPoint nextnode,int numpoint){
		Vector3[] ve3 = new Vector3[4];
		if (beizerMode == BeizerMode.Beizer || beizerMode == BeizerMode.Free) {
			ve3 = CalculateBeizer (this, nextnode, numpoint);
		}
		else {
			ve3 = new Vector3[2];
			ve3 [0] = Point;
			ve3 [1] = nextnode.Point;
		}	
		Length = 0;
		for (int i = 0; i < ve3.Length - 1; i++) {
			Length += Vector3.Distance(ve3[i],ve3[i+1]);
		}
		return ve3;
	}
	public Vector3 GetPointWithProportion(float t,BeizerPoint nextBeizer){
		Vector3 tPoint;   
		if (beizerMode == BeizerMode.Beizer || beizerMode == BeizerMode.Free) {
			
			tPoint.x = MetaComputing (Point.x, NextControlPoint.x, nextBeizer.BackControlPoint.x, nextBeizer.Point.x, t);  
			tPoint.y = MetaComputing (Point.y, NextControlPoint.y, nextBeizer.BackControlPoint.y, nextBeizer.Point.y, t);  
			tPoint.z = MetaComputing (Point.z, NextControlPoint.z, nextBeizer.BackControlPoint.z, nextBeizer.Point.z, t);  
		} else {
			tPoint = Point + ((nextBeizer.Point - Point)*t);
		}
		return tPoint;   
	}
	// CalculateBeizer 以控制点 cp 所产生的曲线点，填入 Point_Float 结构数组。   
	// 调用方必须分配足够的空间以供输出，<sizeof(Point_Float) numOfPoints>  
	static Vector3[] CalculateBeizer(BeizerPoint startpoint,BeizerPoint nextpoint, int numOfPoints)  
	{  
		
		float t;  
		int i;   
		t = 1.0f/(numOfPoints - 1);  
		Vector3[] curve = new Vector3[numOfPoints];
		for(i = 0; i < numOfPoints; i++)   
			curve[i] = PointOnCubicBezier(startpoint,nextpoint, i*t);  
		return curve;
	}  
	
	// 参数1: 4个点坐标(起点，控制点1，控制点2，终点)  
	// 参数2: 0 <= t <= 1   
	static Vector3 PointOnCubicBezier(BeizerPoint sp,BeizerPoint np, float t)   
	{   
		Vector3 tPoint;   
		tPoint.x = MetaComputing(sp.Point.x, sp.NextControlPoint.x, np.BackControlPoint.x, np.Point.x, t);  
		tPoint.y = MetaComputing(sp.Point.y, sp.NextControlPoint.y, np.BackControlPoint.y, np.Point.y, t);  
		tPoint.z = MetaComputing(sp.Point.z, sp.NextControlPoint.z, np.BackControlPoint.z, np.Point.z, t);  
		return tPoint;   
	}   
	
	static float MetaComputing(float p0, float p1, float p2, float p3, float t)  
	{  
		// 方法一:  
		float a, b, c;   
		float tSquare, tCube;   
		// 计算多项式系数    
		c = 3.0f * (p1 - p0);   
		b = 3.0f * (p2 - p1) - c;   
		a = p3 - b - c - p0;   
		
		// 计算t位置的点   
		tSquare = t * t;   
		tCube   = t * tSquare;   
		return (a * tCube) + (b * tSquare) + (c * t) + p0;  
		
	}  
}
[System.Serializable]
public class BeizerCurver{
	public BeizerCurver(){
		Points = new List<BeizerPoint> ();
		Points.Add (new BeizerPoint (Vector3.down));
		Points.Add(new BeizerPoint(Vector3.up));
		GetPathLength ();
	}
	public List<BeizerPoint> Points;
	public float PathLength;
	public float TestProportion;
	public float TimeLength = 1;
	private bool CanDrawBeizer{
		get{ return Points.Count > 1;}
	}
	public List<Vector3> GetAllPoints(){
		List<Vector3> point = new List<Vector3>();
		for (int i = 0; i < Points.Count -1; i++) {
			Vector3[] nodepoint = Points[i].CalculateBeizer(Points[i+1],20);
			point.AddRange(nodepoint);
		}
		return point;
	}
	public void CalculateBeizerLength(){
		for (int i = 0; i < Points.Count -1; i++) {
			Points[i].CalculateBeizer(Points[i+1],20);
		}
	}
	private void DrawPath(List<Vector3> point){
		Gizmos.color = Color.white;
		for (int i = 0; i < point.Count - 1; i++) {
			Gizmos.DrawLine(point[i],point[i+1]);
		}
	}
	public float GetPathLength(){
		if (CanDrawBeizer) {
			PathLength = 0;
			for(int i = 0;i < Points.Count;i++){
				PathLength += Points[i].Length;
			}
			if(PathLength!=0){
				for(int i = 0;i < Points.Count;i++){
					Points[i].Proportion = (Points[i].Length/PathLength+GetPointProporiton(i-1));
				}
			}
		}
		return PathLength;
	}
	private float GetPointProporiton(int index){
		if (index >= 0)
			return Points [index].Proportion;
		else
			return 0;
	}
	public int GetPointIndexWithProporiton(float f){
		for (int i = 0; i < Points.Count - 1; i++) {
			if( Points[i].Proportion >= f){
				return i;
			}
		}
		return Points.Count - 2	;
//		if (Points.Count > 2)
//			return Points.Count - 3;
//		else
//			return Points.Count - 2;
	}
	public float AllProportionAsPointProportion(float value){
		GetP_index = Mathf.Clamp (GetPointIndexWithProporiton (value), 0, Points.Count - 2);
		if (GetP_index == Points.Count - 2 && GetP_index > 0) {
			GetP_offset = Points [GetP_index].Proportion - Points [GetP_index - 1].Proportion;
			GetP_f = 1 - (Mathf.Max (0, Points [GetP_index].Proportion) - value) / GetP_offset;
		} else {
			if (GetP_index > 0)
				GetP_offset = Points [GetP_index].Proportion - Points [GetP_index - 1].Proportion;
			else
				GetP_offset = Points [GetP_index].Proportion;
			GetP_f = 1 - (Points [GetP_index].Proportion - value) / GetP_offset;
		}
		return GetP_f;
	}
	float GetP_f;
	float GetP_offset; 
	int GetP_index;
	public float qoffset = 0.1f;
	public Vector3 GetProportion(float value,bool clamp = true){
		if (CanDrawBeizer) {
			value = Normalize (value,clamp);
			GetP_index = Mathf.Clamp (GetPointIndexWithProporiton (value), 0, Points.Count - 2);
		
			if (GetP_index == Points.Count - 2 && GetP_index>0) {
				GetP_offset = Points [GetP_index].Proportion - Points [GetP_index - 1].Proportion;
				GetP_f = 1 - (Mathf.Max (0, Points [GetP_index].Proportion) - value) / GetP_offset;
				return Points [GetP_index].GetPointWithProportion (GetP_f, Points [GetP_index + 1]);
			} else {
				if (GetP_index > 0)
					GetP_offset = Points [GetP_index].Proportion - Points [GetP_index - 1].Proportion;
				else
					GetP_offset = Points [GetP_index].Proportion;
				GetP_f = 1 - (Points [GetP_index].Proportion - value) / GetP_offset;
				return Points [GetP_index].GetPointWithProportion (GetP_f, Points [GetP_index + 1]);
			}

		} else {
			return Vector3.zero;
		}
	}
	public float MaxClamp = -1;
	private float Normalize(float proportion,bool clamp){
		if (clamp) {
			proportion = Mathf.Clamp (proportion, 0, 1);
		} else {
			if(MaxClamp == -1)
				proportion = Mathf.Max (proportion, 0);
			else
				proportion = Mathf.Clamp (proportion, 0, MaxClamp);
		}
		return proportion;
	}
	public Vector3 GetPosition(float proporition,bool clamp = true){
		TestProportion = proporition;
		return GetProportion (proporition,clamp);
	}
	public float GetQOffest(float proporition){
		TestProportion = proporition;
		return Mathf.Min (proporition, qoffset);
	}
	public Vector3 GetLookAt(float proporition){
		TestProportion = proporition;
		return getLookAt (GetProportion(Mathf.Max (0, proporition - GetQOffest(proporition))),GetProportion(Mathf.Min (0, proporition + GetQOffest(proporition))));
	}
	public Quaternion GetQuaternion(float proporition){
		TestProportion = proporition;
		return GetMiddleQuaternion (GetProportion(Mathf.Max (0, proporition - GetQOffest(proporition))),GetProportion(Mathf.Min (0, proporition + GetQOffest(proporition))));
	}
	private Vector3 getLookAt(Vector3 point1,Vector3 point2){
		Vector3 newp1 = point2 - point1;
		Vector3 np= Vector3.zero;
		Vector3 f = Vector3.back;
		Vector3.OrthoNormalize (ref newp1,ref f,ref np);
		return np;
	}
	/// <summary>
	/// 获取两个点中间的旋转
	/// </summary>
	/// <returns>The middle quaternion.</returns>
	/// <param name="point1">Point1.</param>
	/// <param name="point2">Point2.</param>
	private Quaternion GetMiddleQuaternion(Vector3 point1,Vector3 point2){
		if (point1 == Vector3.zero && point2 == Vector3.zero)
			return new Quaternion();; 
		Vector3 newp1 = point2 - point1;
		Vector3 np= Vector3.zero;
		Vector3 f = Vector3.back;
		Vector3.OrthoNormalize (ref newp1,ref f,ref np);
		Quaternion nq = new Quaternion();
		if ((((point2 - point1) * 0.5f).normalized.normalized + np).normalized == Vector3.zero) {
			return nq;
		}
		if (((point2 - point1) * 0.5f).normalized == Vector3.zero) {
			return nq;
		}
		nq.SetLookRotation (((point2 - point1)*0.5f).normalized,(((point2 - point1)*0.5f).normalized.normalized + np).normalized);
		return nq;
	}
	/// <summary>
	/// 获取两个点中间的位置
	/// </summary>
	private Vector3 GetMiddlePosition(Vector3 point1,Vector3 point2){
		return (point1 + point2) * 0.5f;
	}
	public Quaternion GetTangentQ(float p){
		Quaternion q = Quaternion.LookRotation (GetTangent (p));
		return q;
	}
	/// <summary>
	/// 获取切线
	/// </summary>
	/// <returns>The tangent.</returns>
	/// <param name="p">P.</param>
	public Vector3 GetTangent(float p){
		int index = GetPointIndexWithProporiton (p);
		float propor = AllProportionAsPointProportion (p);
		Vector3 p_h = GetTwoPoint(Points [index].NextControlPoint,Points [index].Point,propor);
		Vector3 p_g = GetTwoPoint(Points [index+1].BackControlPoint,Points [index].NextControlPoint,propor);
		Vector3 p_j = GetTwoPoint(p_g,p_h,propor);
		Vector3 p_f = GetTwoPoint(Points [index + 1].Point, Points [index + 1].BackControlPoint, propor);
		Vector3 p_i = GetTwoPoint (p_f, p_g, propor);
		
		return (p_i - p_j).normalized	;
	}
	public Vector3 GetTwoPoint(Vector3 p1,Vector3 p2,float p){
		return (p1 -p2)*p+p2;
	}

	public void OnDrawGizmos(){
		Gizmos.DrawSphere(GetProportion(TestProportion),0.1f);
	}

	public void Insert(int index,float proportion = 0.5f){
		index = Mathf.Clamp (index, 0, Points.Count-2);
		BeizerPoint point;
		if (index < Points.Count - 1) {
			point = new BeizerPoint (Points [index].GetPointWithProportion (proportion, Points [index + 1]));
			Vector3 p1;
			Vector3 p2;
			Vector3 p3;
			Vector3 p4;
			Vector3 p5;

			p1 = (Points[index].NextControlPoint - Points[index].Point)*proportion+Points[index].Point;
			p2 = (Points[index + 1].BackControlPoint - Points[index].NextControlPoint)*proportion+Points[index].NextControlPoint;
			p3 = (p2 - p1)*proportion+p1;

			p4 = (Points[index + 1].Point - Points[index + 1].BackControlPoint)*proportion+Points[index+1].BackControlPoint;
			p5 = (p4 - p2)*proportion+p2;

			Points[index].NextControlPoint = p1;
		    point.BackControlPoint = p3;
			point.NextControlPoint = p5;
			Points[index+1].BackControlPoint = p4;

		}
		else{
			point = new BeizerPoint (Points [index-2].GetPointWithProportion(proportion,Points [index-1]));
		}
		Points.Insert(Mathf.Max(0,index+1),point);
	}
	public void Dele(int id){
		if (Points.Count > 2)
			Points.RemoveAt (id);
	}
}


