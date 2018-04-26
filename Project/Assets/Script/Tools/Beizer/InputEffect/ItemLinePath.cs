using UnityEngine;
using System.Collections;
[System.Serializable]
public class LinePoints{

	private Vector3 m_PointValue = Vector3.zero;
	public float time;
	public Vector3 PointValue{
		get{ 
			return m_PointValue;
		}
		set{ 
			m_PointValue = value;

		}
	}
	
	/// <summary>
	/// 进度条
	/// </summary>
	public float Proportion;
}
[System.Serializable]
public class ItemLine{
	private LinePoints[] Points;
	private LineRenderer Line;
	private int Count;
	private float timesave;
	public ItemLine(Transform target,int count){
		Line = target.GetComponent<LineRenderer> ();
		if (Line == null) {
			Line = target.gameObject.AddComponent<LineRenderer>();
		}
		Count = count;
		Line.SetVertexCount (Count);
		Points = new LinePoints[count];
		for (int i = 0; i < count; i++) {
			Points[i] = new LinePoints();
			Line.SetPosition(i,Vector3.zero);
		}
	}
	public void SetCount(int count){
		Line.SetVertexCount (Count);
	}
	public  void UpdatePointProportion(float time,float offest,float MoveSpeed,bool lockEnd,float MoveLength){
		if (time == 0)
			return;
		int count = Count;
		if (lockEnd) {
			Points [count - 1].Proportion = 0;
			count --;
		}
		for (int i =0; i < count; i++) {
			Points [i].Proportion = time*MoveSpeed-(i+1)*offest*MoveLength;
		}
	}
	public void UpdateLinePos(BeizerCurver curver,float RandomV,float time,float offset){
		for (int i =0; i < Count; i++) {
			SetPoint(curver,i,Points[i].Proportion,RandomV,time,offset);
			Line.SetPosition(i,Points[i].PointValue);
		}
	}
	public void UpdateWidth(float time,float endwidth,float startwidth,float movetime){
			time = Mathf.Clamp (time / movetime, 0, 1);
		Line.SetWidth (Mathf.Lerp (0, startwidth, time), (startwidth -endwidth)*Points[Points.Length - 1].Proportion);
	}
	public void SetPoint(BeizerCurver curver,int index,float proportion){
		Vector3 pos = curver.GetPosition(proportion);
		Points [index].PointValue = pos;
		for (int i =0; i < Count; i++) {
			Line.SetPosition(i,Points[i].PointValue);
		}
	}
	public void SetPoint(BeizerCurver curver,int index,float proportion,float RandomV,float time,float offset){
		Vector3 pos = curver.GetPosition(proportion);
		if (proportion > offset) {
				Vector3 qiexian = curver.GetTangent (proportion);
				Vector3 chuizhi = Random.insideUnitSphere*RandomV;
				Vector3.OrthoNormalize (ref qiexian, ref chuizhi);
			pos+= chuizhi * proportion * RandomV;
		}
		Points [index].PointValue = pos;
	}
	public void Show(){
		Line.gameObject.SetActive (true);
		for(int i = 0;i<Count;i++){
			Line.SetPosition(i,Vector3.zero);
		}
	}
	public void Hide(){
		Line.gameObject.SetActive (false);
	}
}
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(BeizerLine))]
[ExecuteInEditMode]
public class ItemLinePath : MonoBehaviour {
	BeizerLine _line;
	ItemLine _itemLine;
	public int _lineCount = 30;
	public bool _updateLine;
	float _lineProportion;
	public bool updateNow = false;
	public void Start(){
		_line = GetComponent<BeizerLine> ();
		_itemLine = new ItemLine (transform, _lineCount+1);

		DrawLine ();
	}

	void Update(){
		if (_updateLine)
			DrawLine ();
		if (updateNow) {
			updateNow = false;
			DrawLine ();
		}
	}
	void DrawLine(){
		_itemLine.SetCount (_lineCount);
		_lineProportion = 1f / _lineCount;
		for (int i = 0; i < _lineCount; i++) {
			_itemLine.SetPoint (_line.Curver, i, _lineProportion * i);
		}
		_itemLine.SetPoint (_line.Curver, _lineCount, 1);
	}

}
