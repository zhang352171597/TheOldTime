using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace zPhoneDebugTool
{

	[Serializable]
	public class GameObjectProperty
	{
		public string name;
		public GameObject root;
		public Behaviour behavior;

		public GameObjectProperty (Behaviour _behavior)
		{
			behavior = _behavior;
		}

		public void OnGUI ()
		{
			GUILayout.BeginHorizontal (zPhoneDebugTool.titleStyle);
			GUILayout.Label (behavior.GetType ().ToString ());
			GUILayout.EndHorizontal ();



			var propertys = behavior.GetType ().GetProperties (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < propertys.Length; i++) {
				if (!propertys [i].CanRead)
					continue;

				try {
					var _v = zPhoneDebugTool.DrawProperty (propertys [i].Name, propertys [i].PropertyType, propertys [i].GetValue (behavior, null));
					propertys [i].SetValue (behavior, _v, null);
				} catch {
				}
			}


			var fieldInfo = behavior.GetType ().GetFields (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int i = 0; i < fieldInfo.Length; i++) {
				try {
					var _v = zPhoneDebugTool.DrawProperty (fieldInfo [i].Name, fieldInfo [i].FieldType, fieldInfo [i].GetValue (behavior));
					fieldInfo [i].SetValue (behavior, _v);
				} catch {

				}
			}
		}



	}

	[Serializable]
	public class GameObjectBehavior
	{
		public string name;
		public GameObject root;
		public Behaviour behavior;
		GameObjectProperty _propertys;

		GameObjectProperty propertys {
			get {
				if (_propertys == null)
					_propertys = new GameObjectProperty (behavior);

				return _propertys;
			}
		}


		public void OnGUI ()
		{
			if (zPhoneDebugTool.DrawButton (behavior.GetType ().ToString (), Color.black, false))
				zPhoneDebugTool.Instance.hierarchyInspector.curChooseProperty = propertys;
		}

		public GameObjectBehavior (GameObject _root, Behaviour _behavior)
		{
			root = _root;
			behavior = _behavior;
		}



		public Behaviour GetBehaviors ()
		{
			return behavior;
		}
	}

	[Serializable]
	public class GameObjectData
	{
		public string name;
		public GameObject root;
		public GameObjectDataList childs;
		public GameObjectDataList dataList;
		public bool foldout;

		List<GameObjectBehavior> _behaviors;

		List<GameObjectBehavior> behaviors {
			get {
				if (_behaviors == null) {
					_behaviors = new List<GameObjectBehavior> ();
					var tBehaviors = root.GetComponents<Behaviour> ();
					for (int i = 0; i < tBehaviors.Length; i++) {
						var gob = new GameObjectBehavior (root, tBehaviors [i]);
						_behaviors.Add (gob);
					}
				}

				return _behaviors;
			}
		}

		public void OnGUI ()
		{
			GUILayout.BeginHorizontal ();
			if (zPhoneDebugTool.DrawButton (root.activeSelf ? "隐藏" : "显示", GUILayout.Width(40.0f)))
				root.SetActive(!root.activeSelf);
				

			if (zPhoneDebugTool.DrawButton (name + " : " + (root.activeSelf ? "true" : "false"), Color.yellow, false))
				OnSelectGameObject ();

			if (childs.GetGOCount () > 0) {
				if (zPhoneDebugTool.DrawButton ("下钻", Color.black))
					zPhoneDebugTool.Instance.hierarchyInspector.DrillDown (childs);
			}

			GUILayout.EndHorizontal ();
		}

		public void OnSelectGameObject ()
		{
			zPhoneDebugTool.Instance.hierarchyInspector.curChooseBehavior = behaviors;
		}
	}

	[Serializable]
	public class GameObjectDataList
	{
		public List<GameObjectData> datas;

		public GameObjectDataList ()
		{
			datas = new List<GameObjectData> ();
		}

		public void AddData (GameObjectData _data)
		{
			datas.Add (_data);
			_data.dataList = this;
		}

		public void AddChildData (int index, GameObjectDataList _childData)
		{
			datas [index].childs = _childData;
		}

		public int GetGOCount ()
		{
			return datas.Count;
		}

		public Transform GetRoot (int _index)
		{
			return datas [_index].root.transform;
		}

		public GameObjectData GetData(int _index){
			return datas [_index];
		}

		public void OnGUI ()
		{
			for (int i = 0; i < datas.Count; i++) {
				datas [i].OnGUI ();
			}
		}

		public void Clear(){
			datas.Clear ();
		}
	}


	public class zPhoneDebugTool : MonoBehaviour
	{
		public enum ShowType
		{
			/// <summary>
			/// 日记
			/// </summary>
			enLog,
			/// <summary>
			/// 物体
			/// </summary>
			enHierarchy
		}

		#region Tool Member

		public static float ButtonWidth = 50.0f;
		public static float ButtonHeight = 35.0f;
		public static float ButtonMaxWidth = 400.0f;


		static GUIStyle _noBGButtonStyle;

		public static GUIStyle NoBGButtonStyle {
			get { 
				if (_noBGButtonStyle == null) {
					_noBGButtonStyle = new GUIStyle ();
					_noBGButtonStyle.normal.textColor = Color.black;
					_noBGButtonStyle.alignment = TextAnchor.MiddleCenter;
				}

				return _noBGButtonStyle;
			}
		}

		static GUIStyle _winbg;

		static public GUIStyle winbg {
			get {
				if (_winbg == null) {
					_winbg = new GUIStyle ();
					Texture2D texture = new Texture2D (2, 2);
					SetTextureColor (texture, new Color (0.5f, 0.5f, 0.5f, 1));
					_winbg.normal.background = texture;

				}
				return _winbg;
			}
		}

		static GUIStyle _titleStyle;

		public static GUIStyle titleStyle {
			get { 
				if (_titleStyle == null) {
					_titleStyle = new GUIStyle ();
					Texture2D texture = new Texture2D (2, 2);
					SetTextureColor (texture, Color.blue);
					_titleStyle.normal.background = texture;
				}

				return _titleStyle;
			}
		}

		static GUIStyle _nonBtnSytle;

		public static GUIStyle nonBtnStyle {
			get { 
				if (_nonBtnSytle == null) {
					_nonBtnSytle = new GUIStyle ();
					_nonBtnSytle.normal.textColor = Color.blue;
				}

				return _nonBtnSytle;
			}
		}

		#endregion

		static zPhoneDebugTool _instance;

		public static zPhoneDebugTool Instance {
			get {
				if (_instance == null) {
					_instance = GameObject.FindObjectOfType<zPhoneDebugTool> ();
					GameObject.DontDestroyOnLoad (_instance);
				}
				if (_instance == null) {
					_instance = new GameObject ("zPhoneDebugTool").AddComponent<zPhoneDebugTool> ();
					GameObject.DontDestroyOnLoad (_instance);
				}

				return _instance;
			}
		}

		public const int indentCount = 2;

		Vector2 inspectorVe2;
		bool showPhoneDebug;
		bool stopApplication;
		float cachedTimeScale;

		public ShowType showType;
		DiaryLog diaryLog;

        HierarchyInspector _hierarchyInspector;
		public HierarchyInspector hierarchyInspector
        {
            get
            {
                if(_hierarchyInspector == null)
                    _hierarchyInspector = new HierarchyInspector();
                return _hierarchyInspector;
            }
        }

		void Awake ()
		{
			Instance.Init ();
		}

		bool inited;
		public void Init ()
		{
			if (inited)
				return;
			inited = true;
			diaryLog = new DiaryLog ();
		}

		void OnEnable ()
		{
            hierarchyInspector.OnEnable();
			showType = ShowType.enHierarchy;
		}


		void OnGUI ()
		{
			if (!showPhoneDebug && GUILayout.Button ("", nonBtnStyle, GUILayout.Width (30), GUILayout.Height (30))) {
				showPhoneDebug = true;
				stopApplication = true;
				cachedTimeScale = Time.timeScale;
				Time.timeScale = 0;
			}

			if (!showPhoneDebug)
				return;

//			if (Time.frameCount % 20 == 0) {
//				Debug.LogError ("errorLog");
//			}


			GUILayout.BeginHorizontal ();
			if (DrawButton ("隐藏", Color.black))
				showPhoneDebug = false;

			if (DrawButton ("刷新", Color.black)) {
				hierarchyInspector.UpdateHierarchyGo = true;
			}

			if (DrawButton (stopApplication ? "继续" : "暂停", Color.black)) {
				stopApplication = !stopApplication;
				if (stopApplication) {
					Time.timeScale = 0;
				} else
					Time.timeScale = 1;
			}

			if (showType != ShowType.enHierarchy && hierarchyInspector.drawHierachyTitle ())
				showType = ShowType.enHierarchy;
		
			if (showType != ShowType.enLog && diaryLog.drawDiaryTitle ())
				showType = ShowType.enLog;

			if (showType == ShowType.enHierarchy) {
				hierarchyInspector.OnGUI ();
			} else if (showType == ShowType.enLog) {
				diaryLog.OnGUI ();
			}

			GUILayout.EndHorizontal ();



		}

		#region Tools

		public static object DrawProperty (string _propertyName, System.Type _type, object _value)
		{
			switch (_type.ToString ()) {
			case "System.String":
			case "System.Single":
			case "System.Double":
			case "System.Int32":
				GUILayout.BeginHorizontal ();
				GUILayout.Label (_propertyName);
				_value = GUILayout.TextField (_value == null ? "null" : _value.ToString ());
				GUILayout.EndHorizontal ();
				break;
			case "System.Boolean":
				_value = GUILayout.Toggle ((bool)_value, _propertyName);
				break;

			case "UnityEngine.Vector2":
				GUILayout.BeginHorizontal ();
				float tempf;
				Vector2 v2 = (Vector2)_value;
				GUILayout.Label (_propertyName);
				GUILayout.Label ("x:");
				string strx = GUILayout.TextField (v2.x.ToString ());
				if (float.TryParse (strx, out tempf))
					v2.x = float.Parse (strx);
				GUILayout.Label ("y:");

				string stry = GUILayout.TextField (v2.y.ToString ());
				if (float.TryParse (stry, out tempf))
					v2.y = float.Parse (stry);
			
				GUILayout.EndHorizontal ();
				_value = v2;
				break;
			case "UnityEngine.Vector3":
				GUILayout.BeginHorizontal ();
				float v3tempf;

				Vector3 v3 = (Vector3)_value;
				GUILayout.Label (_propertyName);
				GUILayout.Label ("x:");
				string v3strx = GUILayout.TextField (v3.x.ToString ());
				if (float.TryParse (v3strx, out v3tempf))
					v3.x = float.Parse (v3strx);

				GUILayout.Label ("y:");
				string v3stry = GUILayout.TextField (v3.y.ToString ());
				if (float.TryParse (v3stry, out v3tempf))
					v3.y = float.Parse (v3stry);

				GUILayout.Label ("z:");
				string v3strz = GUILayout.TextField (v3.z.ToString ());
				if (float.TryParse (v3strz, out v3tempf))
					v3.z = float.Parse (v3strz);

				GUILayout.EndHorizontal ();
				_value = v3;
				break;

			case "UnityEngine.Transform":
			case "UnityEngine.GameObject":
				break;

			case "System.Action":
				break;
			default:
//				Debug.Log ("未处理类型  :[" + _type.ToString ());
				break;
			
			}
			return _value;
		}

		/// <summary>
		/// 默认的宽高
		/// </summary>
		/// <returns><c>true</c>, if button was drawn, <c>false</c> otherwise.</returns>
		/// <param name="_content">内容.</param>
		/// <param name="_color">颜色.</param>
		/// <param name="_drawBG">是否画背景 <c>true</c> draw B.</param>
		public static bool DrawButton (string _content, Color _color = default(Color), bool _drawBG = true)
		{
			GUI.backgroundColor = _color;

			bool result = false;
			if (_drawBG)
				result = GUILayout.Button (_content, GUILayout.Width (ButtonWidth), GUILayout.Height (ButtonHeight));
			else
				result = GUILayout.Button (_content, NoBGButtonStyle, GUILayout.Width (ButtonWidth), GUILayout.Height (ButtonHeight), GUILayout.MaxWidth (ButtonMaxWidth));


			GUI.backgroundColor = Color.white;

			return result;
		}

		/// <summary>
		/// 自定义GUILayoutOption 参数
		/// </summary>
		/// <returns><c>true</c>, if button was drawn, <c>false</c> otherwise.</returns>
		/// <param name="_content">Content.</param>
		/// <param name="_color">Color.</param>
		/// <param name="_drawBG">If set to <c>true</c> draw B.</param>
		/// <param name="_options">Options.</param>
		public static bool DrawButton (string _content, params GUILayoutOption[] _options)
		{
			GUI.backgroundColor = Color.black;
			bool result;
			result = GUILayout.Button (_content, _options);

			GUI.backgroundColor = Color.white;

			return result;
		}


		/// <summary>
		/// 递归查找子节点
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="root">Root.</param>
		public static GameObjectDataList findChild (Transform _root)
		{
//		List<GameObjectData> result = new List<GameObjectData> ();
			GameObjectDataList result = new GameObjectDataList ();

			for (int i = 0; i < _root.childCount; i++) {
				var tData = new GameObjectData ();
				tData.name = _root.GetChild (i).name;
				tData.root = _root.GetChild (i).gameObject;
//			tData.childs = findChild (tData.root.transform);
				result.AddData (tData);
				result.AddChildData (i, findChild (result.GetRoot (i)));
//			result.Add (tData);
			}

			return result;
		}

		public static void SetTextureColor (Texture2D texture, Color color)
		{
			for (int i = 0; i < texture.width; i++) {
				for (int x = 0; x < texture.height; x++) {
					texture.SetPixel (i, x, color);
				}
			}
		}

		#endregion
	}


	public class DiaryLog
	{
		#region Member

		Rect diaryRect = new Rect (0, 50, Screen.width, Screen.height - 50);
		const int diaryId = 10;

		GUIStyle _normalStyle;

		GUIStyle normalStyle {
			get {
				if (_normalStyle == null) {
					_normalStyle = new GUIStyle ();
					_normalStyle.fontSize = 16;
					_normalStyle.normal.textColor = Color.white;
				}
				return _normalStyle;
			}
		}

		GUIStyle _warningStyle;

		GUIStyle warningStyle {
			get {
				if (_warningStyle == null) {
					_warningStyle = new GUIStyle ();
					_warningStyle.fontSize = 16;
					_warningStyle.normal.textColor = Color.yellow;
				}
				return _warningStyle;
			}
		}

		GUIStyle _errorStyle;

		GUIStyle errorStyle {
			get { 
				if (_errorStyle == null) {
					_errorStyle = new GUIStyle ();
					_errorStyle.fontSize = 16;
					_errorStyle.normal.textColor = Color.red;
				}

				return _errorStyle;
			}
		}

		bool showNormal = true;
		bool showWarning;
		bool showError;
		bool receiveLog = true;

		#endregion


		public DiaryLog ()
		{
			Application.logMessageReceived += onAddLog;
		}

		#region Private

		/// <summary>
		/// 画日记的头部
		/// </summary>
		public bool drawDiaryTitle ()
		{
			bool result = zPhoneDebugTool.DrawButton ("显示日记", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight));
			return result;
		}

		#endregion


		#region Interface

		Vector2 diaryScrollVe2;

		public void OnGUI ()
		{
			zPhoneDebugTool.ShowType _showtype = zPhoneDebugTool.Instance.showType;
			if (_showtype == zPhoneDebugTool.ShowType.enLog && zPhoneDebugTool.DrawButton ("清理", Color.black))
				logDatas.Clear ();
			if (_showtype == zPhoneDebugTool.ShowType.enLog && zPhoneDebugTool.DrawButton ((receiveLog ? "不接收" : "接收") + "日记", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight)))
				receiveLog = !receiveLog;
			if (_showtype == zPhoneDebugTool.ShowType.enLog && zPhoneDebugTool.DrawButton ((showNormal ? "显示" : "隐藏") + "普通", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight)))
				showNormal = !showNormal;
			if (_showtype == zPhoneDebugTool.ShowType.enLog && zPhoneDebugTool.DrawButton ((showWarning ? "显示" : "隐藏") + "警告", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight)))
				showWarning = !showWarning;
			if (_showtype == zPhoneDebugTool.ShowType.enLog && zPhoneDebugTool.DrawButton ((showError ? "显示" : "隐藏") + "错误", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight)))
				showError = !showError;


			GUI.Window (diaryId, diaryRect, delegate(int id) {
				GUILayout.BeginVertical ();

				diaryScrollVe2 = GUILayout.BeginScrollView (diaryScrollVe2);

				for (int i = 0; i < logDatas.Count; i++) {
					if (logDatas [i].IsType (LogType.Log) && showNormal) {
						GUILayout.Label (logDatas [i].GetContent (), normalStyle);
					} else if (logDatas [i].IsType (LogType.Warning) && showWarning) {
						GUILayout.Label (logDatas [i].GetContent (), warningStyle);
					} else if (logDatas [i].IsType (LogType.Error) && showError) {
						GUILayout.Label (logDatas [i].GetContent (), errorStyle);
					}

				}

				GUILayout.EndScrollView ();

				GUILayout.EndVertical ();
			}, "日记");
		}

		#endregion

		List<LogData> logDatas = new List<LogData> ();

		void onAddLog (string condition, string stackTrace, LogType type)
		{
			if (!receiveLog)
				return;

			LogData _data = new LogData (condition, stackTrace, type);
			logDatas.Add (_data);
		}

		#region Class

		public class LogData
		{
			LogType type;
			string content;

			public LogData (string _content, string _stackTrace, LogType _type)
			{
				content = _content + "\n" + _stackTrace;
				type = _type;
			}

			public bool IsType (LogType _type)
			{
				return type == _type;
			}

			public string GetContent ()
			{
				return content;
			}
		}

		#endregion
	}


	public class HierarchyInspector
	{
		#region Member

		public GameObjectDataList hierarchyGO = new GameObjectDataList ();

		GameObjectDataList curChooseGO;

		List<GameObjectDataList> cachedGO = new List<GameObjectDataList> ();

		public List<GameObjectBehavior> curChooseBehavior = new List<GameObjectBehavior> ();
		public GameObjectProperty curChooseProperty;


		public bool UpdateHierarchyGo;

		Rect GOwindowRect = new Rect (0, 50, Screen.width / 3, Screen.height - 50);
		Rect BehaviorWindowRect = new Rect (Screen.width / 3 + 20, 50, Screen.width / 3 - 20, Screen.height - 50);
		Rect InspectorWindowRect = new Rect ((Screen.width / 3) * 2 + 20, 50, Screen.width / 3 - 20, Screen.height - 50);
		Vector2 gameObjectVe2;
		Vector2 behaviourVe2;
		Vector2 inspectorVe2;

		#endregion

		public HierarchyInspector ()
		{
			initHierarchyGO ();
		}

		#region Public

		public void OnEnable ()
		{
			UpdateHierarchyGo = true;
			curChooseGO = hierarchyGO;	
		}

		/// <summary>
		/// 画物体的头部
		/// </summary>
		public bool drawHierachyTitle ()
		{
			bool result = zPhoneDebugTool.DrawButton ("显示物体", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight));
			return result;
		}

		public void OnGUI ()
		{
			if (zPhoneDebugTool.DrawButton ("显示相机", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight))) {
				showCamera ();
			}

			if (cachedGO.Count > 0) {
				if (zPhoneDebugTool.DrawButton ("返回上一级", GUILayout.Width (150), GUILayout.Height (zPhoneDebugTool.ButtonHeight)))
					DrillUp ();
			}

			updateHierarchyGO ();

			drawGameObejct ();
			drawBehavior ();
			drawInspector ();


		}

		/// <summary>
		/// 下钻
		/// </summary>
		/// <param name="_go">Go.</param>
		public void DrillDown (GameObjectDataList _go)
		{
			var temp = curChooseGO;
			addCachedGO (temp);
			curChooseGO = _go;	
		}

		/// <summary>
		/// 上钻
		/// </summary>
		public void DrillUp ()
		{
			curChooseGO = pushCachedData ();
		}

		#endregion

		#region Private

		/// <summary>
		/// 添加缓存当前场景物体
		/// </summary>
		/// <param name="_data">Data.</param>
		void addCachedGO (GameObjectDataList _data)
		{
			cachedGO.Add (_data);
		}

		/// <summary>
		/// 弹出缓存当前场景物体
		/// </summary>
		/// <returns>The cached data.</returns>
		GameObjectDataList pushCachedData ()
		{
			if (cachedGO.Count == 0)
				return null;

			var result = cachedGO [cachedGO.Count - 1];
			cachedGO.RemoveAt (cachedGO.Count - 1);
			return result;
		}


		/// <summary>
		/// 更新层级物体
		/// </summary>
		void updateHierarchyGO ()
		{
			if (hierarchyGO.GetGOCount () <= 0)
				return;
			
			if (hierarchyGO.GetData(0).root == null) {
				UpdateHierarchyGo = true;
			}

			if (UpdateHierarchyGo) {
				initHierarchyGO ();
				for (int i = 0; i < hierarchyGO.GetGOCount (); i++) {
					hierarchyGO.AddChildData (i, findChild (hierarchyGO.GetRoot (i)));
				}
				UpdateHierarchyGo = false;
			}
		}

		/// <summary>
		/// 初始添加层级物体
		/// </summary>
		void initHierarchyGO ()
		{
			hierarchyGO.Clear ();
			GameObject[] gos = GameObject.FindObjectsOfType<GameObject> ();
			for (int i = 0; i < gos.Length; i++) {
				if (gos [i].transform.parent == null) {
					var tData = new GameObjectData ();
					tData.name = gos [i].name;
					tData.root = gos [i];
					hierarchyGO.AddData (tData);
				}
			}


		}

		/// <summary>
		/// 显示所有相机
		/// </summary>
		void showCamera(){
			hierarchyGO.Clear ();
			Camera[] gos = GameObject.FindObjectsOfType<Camera> ();
			for (int i = 0; i < gos.Length; i++) {
					var tData = new GameObjectData ();
					tData.name = gos [i].gameObject.name;
					tData.root = gos [i].gameObject;
					hierarchyGO.AddData (tData);
			}

			for (int i = 0; i < hierarchyGO.GetGOCount (); i++) {
				hierarchyGO.AddChildData (i, findChild (hierarchyGO.GetRoot (i)));
			}
		}

		/// <summary>
		/// 画物体
		/// </summary>
		void drawGameObejct ()
		{
			GOwindowRect = GUI.Window (-1, GOwindowRect, delegate(int id) {
				if (curChooseGO == null)
					return;

				GUILayout.BeginVertical ("box");
				gameObjectVe2 = GUILayout.BeginScrollView(gameObjectVe2);

				curChooseGO.OnGUI ();
				GUILayout.EndScrollView();

				GUILayout.EndVertical ();

			}, "GameObject");
		}

		/// <summary>
		/// 画组件
		/// </summary>
		void drawBehavior ()
		{
			BehaviorWindowRect = GUI.Window (1, BehaviorWindowRect, delegate(int id) {
				GUILayout.BeginVertical ();

				behaviourVe2 = GUILayout.BeginScrollView(behaviourVe2);
				for (int i = 0; i < curChooseBehavior.Count; i++) {
					curChooseBehavior [i].OnGUI ();
				}
				GUILayout.EndScrollView();

				GUILayout.EndVertical ();

			}, "Behavior");

		}

		/// <summary>
		/// 画组件属性
		/// </summary>
		void drawInspector ()
		{
			InspectorWindowRect = GUI.Window (2, InspectorWindowRect, delegate(int id) {
				inspectorVe2 = GUILayout.BeginScrollView (inspectorVe2);

				if (curChooseProperty != null)
					curChooseProperty.OnGUI ();

				GUILayout.EndScrollView ();

			}, "Inspector");
		}

		#endregion

		#region Tools

		/// <summary>
		/// 递归查找子节点
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="root">Root.</param>
		public static GameObjectDataList findChild (Transform _root)
		{
			GameObjectDataList result = new GameObjectDataList ();

			for (int i = 0; i < _root.childCount; i++) {
				var tData = new GameObjectData ();
				tData.name = _root.GetChild (i).name;
				tData.root = _root.GetChild (i).gameObject;
				result.AddData (tData);
				result.AddChildData (i, findChild (result.GetRoot (i)));
			}

			return result;
		}

		#endregion

	}
}