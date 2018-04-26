using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIBase : MonoBehaviour
{
    int _depth;
    public int depth
    {get{return _depth;}}

	UIPanel _rootPanel;
	UIPanel rootPanel
	{
		get{
			if (_rootPanel == null)
				_rootPanel = transform.GetComponentInChildren<UIPanel> (true);
			return _rootPanel;
		}
	}
    public void SetDeapth(int depth)
    {
        _depth = depth;
        rootPanel.depth = _depth;
    }
}
public class UIMgr : ModuleComponent<UIMgr> 
{
    int _currentTopDepth;
	Transform _uiRoot;
	public Transform uiRoot
	{
		get{
			if (_uiRoot == null)
            {
                _uiRoot = ObjManager.Instance.addChild(GamePath.UIPrefabs, "UIRoot", transform).transform;
                _currentTopDepth = 0;
                _uiCamera = _uiRoot.GetComponentInChildren<UICamera>().cachedCamera;
            }
			return _uiRoot;
		}
	}
    Camera _uiCamera;
    public Camera uiCamera
    {
        get { return _uiCamera; }
    }
	List<UIBase> _uiList = new List<UIBase>();
	int topDeapth
	{
		get{
            return _currentTopDepth += 10;
		}
	}
    /// <summary>
    /// 输入限制计数器
    /// </summary>
    int _inputLockCounter;
	public T GetUI<T>() where T : UIBase
	{
		var targetName = typeof(T).Name;
		for (int i = 0; i < _uiList.Count; ++i) {
			var tempName = _uiList [i].GetType().Name;
			if (tempName == targetName)
				return _uiList [i] as T;
		}
		var obj = ObjManager.Instance.addChild (GamePath.UIPrefabs, targetName, uiRoot , true);
		var uiCtr = obj.GetComponent<T> ();
        if (typeof(T) == typeof(LoadingUI))
            uiCtr.SetDeapth(999);
        else if (typeof(T) == typeof(HintUI))
            uiCtr.SetDeapth(998);
        else
            uiCtr.SetDeapth(topDeapth);
		uiCtr.transform.localScale = Vector3.one;
        _uiList.Add(uiCtr);
		return uiCtr;
	}
	public void RemoveUI(UIBase uiCtr)
	{
		if (_uiList.Contains (uiCtr)) {
			_uiList.Remove (uiCtr);
            if (uiCtr.depth == _currentTopDepth)
                _currentTopDepth -= 10;
			ObjManager.Instance.Despawn (uiCtr.gameObject);
		}
		else
			Debug.LogWarning (uiCtr.GetType().Name + " want to remove but it is not exist");
	}
    /// <summary>
    /// 修改输入状态
    /// </summary>
    /// <param name="state"></param>
    public void ChangeInputState(bool state)
    {
        if(state)
        {
            _inputLockCounter--;
            if(_inputLockCounter <= 0)
            {
                _inputLockCounter = 0;
                UICamera.ignoreAllEvents = false;
            }
        }
        else
        {
            _inputLockCounter++;
            if (!UICamera.ignoreAllEvents)
                UICamera.ignoreAllEvents = true;
        }
    }
}
