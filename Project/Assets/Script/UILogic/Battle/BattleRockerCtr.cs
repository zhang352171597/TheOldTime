using UnityEngine;
using System.Collections;

/// <summary>
/// 摇杆控制器
/// </summary>
public class BattleRockerCtr : MonoBehaviour
{

	public float rockerRadius = 0.3f;
    public Transform centerPoint;

	bool _touching;
	Vector3 _currentTouchPos;
	Vector3 _currentDir;
    voidDelegateDirection _touchCallback;
    System.Action _touchUpCallback;
	
	public void Bind(voidDelegateDirection touchCallback , System.Action touchUpCallback)
    {
		var touchInfo = UIEventListener.Get(gameObject);
		touchInfo.onPress = _OnPress;
		touchInfo.onDrag = _OnDrag;
		_touchCallback = touchCallback;
        _touchUpCallback = touchUpCallback;
    }

    void OnDisable()
    {
        var touchInfo = UIEventListener.Get(gameObject);
        touchInfo.onPress = null;
        touchInfo.onDrag = null;
    }

	void Update()
	{
		_TouchUpdate ();
	}

    #region TouchAbout
	void _TouchUpdate()
	{
		if (_touching)
			_ReturnDir ();
	}

	void _OnPress(GameObject go , bool state)
    {
		_touching = state;
		if (state) {
			_currentTouchPos = UICamera.currentCamera.ScreenToWorldPoint(UICamera.currentTouch.pos);
			centerPoint.transform.position = _currentTouchPos;
		} else {
			_currentTouchPos = Vector3.zero;
			centerPoint.transform.localPosition = Vector3.zero;
            if (_touchUpCallback != null)
                _touchUpCallback();
		}
    }

    void _OnDrag(GameObject go, Vector2 delta)
    {
		_currentTouchPos = UICamera.currentCamera.ScreenToWorldPoint(UICamera.currentTouch.pos);
		var dis = (int)(Vector3.Distance (_currentTouchPos, transform.position) * 100) / 100.0f;
		var dir = (_currentTouchPos - transform.position).normalized;
		var finalPos = Vector3.zero;
		if (dis > rockerRadius)
			finalPos = transform.position + dir * rockerRadius;
		else
			finalPos = _currentTouchPos;
		centerPoint.transform.position = finalPos;
    }

	void _ReturnDir()
	{
		if (_touchCallback != null) {
            var dir = (centerPoint.transform.position - transform.position).normalized;
			_currentDir.x = dir.x;
			_currentDir.z = dir.y;
			_touchCallback (_currentDir);
		}
	}


    #endregion

}
