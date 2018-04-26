using UnityEngine;
using System.Collections;


public class BeizerHelper {
	/// <summary>
	/// back current next  back current next ```
	/// </summary>
	/// <returns>The point.</returns>
	/// <param name="points">Points.</param>
	static public BeizerPoint[] GetBeizerPoint(params Vector3[] points){
		if (points.Length % 3 != 0) {
			Debug.LogError (" BeizerPoint Params Count Error:" + points.Length);
			return null;
		}
		int count = points.Length/3;
		BeizerPoint[] bp = new BeizerPoint[count];
		for (int i = 0; i < count; i++) {
			bp [i] = new BeizerPoint (points[i*3+1]);
			bp [i].BackControlPoint = points[i * 3];
			bp [i].NextControlPoint = points[i * 3 + 2];
		}
		return bp;
		
	}
	static public Vector3 CalculatePosition(BeizerPoint[] point,float t){
		return getCurver(point).GetPosition(t);
	}
	static public BeizerCurver CalculateParabolaCurver(Vector3 startPos,Vector3 end,Vector3 dir,float hight = 1){
		BeizerPoint[] bp = GetBeizerPoint (startPos, startPos, startPos + hight * dir, end + hight * dir, end, end);
		return getCurver (bp);
	}
	static public Vector3 CalculateParabolaPosition(float t,Vector3 startPos,Vector3 end,Vector3 dir,float hight = 1){
		BeizerPoint[] bp = GetBeizerPoint (startPos, startPos, startPos + hight * dir, end + hight * dir, end, end);
		return getCurver (bp).GetPosition(t);
	}
	
	static public BeizerCurver getCurver(BeizerPoint[] point){
		BeizerCurver curver = new BeizerCurver ();
		curver.Points = new System.Collections.Generic.List<BeizerPoint> (point);
		curver.CalculateBeizerLength ();
		curver.GetPathLength ();
		return curver;
	}

	static public void DrawBoxByCurver(BeizerCurver curver){
		int count = 30;
		for(int i =0;i <count;i++){
			float p = (float)i / count;
			GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
			cube.transform.position = curver.GetPosition (p);
			cube.transform.rotation = curver.GetTangentQ (p);
		}
	}
}
