using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class BeizerLine : MonoBehaviour {
	public BeizerCurver Curver = new BeizerCurver();
	public bool autoPlay = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (autoPlay && !Application.isPlaying) {
			UpdatePos (Curver.TestProportion);
		}
	}
	public void UpdatePos(float p){
		transform.position = Curver.GetPosition (p);
		transform.rotation = Curver.GetTangentQ (p);
	}
}
