using UnityEngine;
using System.Collections;
using System.Threading;
using JSON;
public class NetDemo : MonoBehaviour {
	void Awake(){
		Loom.Initialize ();
	}
	// Use this for initialization
	void Start () {
		SocketManager.GetInstance ().Connect ();

	}
	void HandleNetDemo(JSONNode node){
		Debug.Log (node.ToString ());	
	}	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("1")) {
			ListJsonData data = ListJsonData.Init ();
			data.Add ("UID", 22111);
//			SocketManager.GetInstance ().SendMessage (data.AsJSONNode, NetCommand, HandleNetDemo);
		}	
	}
}