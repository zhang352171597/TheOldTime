using UnityEngine;
using System.Collections;

/// <summary>
/// 网络连接状态检查管理器
/// </summary>
public class NetCheckMgr : ModuleComponent<NetCheckMgr> 
{
	bool missRecv;
	bool _connect;
	bool connect{
		set{
			if (_connect != value) {
				_connect = value;
				if (_connect == false) {
					missRecv = false;
                    _ShowWarningDialog();
				} else {
					_RemoveWaitDialog ();
				}
			}
		}
	}

    Transform _uiTrm;
    Transform uiTrm
    {
        get
        {
            if (_uiTrm == null)
            {
                var uiRoot = GameObject.FindObjectOfType<UIRoot>();
                if (uiRoot != null)
                    _uiTrm = uiRoot.transform;
            }
            return _uiTrm;
        }
    }

    GameObject _touchCutObj;
    GameObject touchCutObj
    {
        get{
            if (_touchCutObj == null)
            {
                /*_touchCutObj = ObjManager.Instance.AddChild(GamePath.COMMONUI, "NetTouchShader", uiTrm);
                _touchCutObj.transform.localPosition = new Vector3(0, 0, 0);
                _touchCutObj.transform.localScale = Vector3.one;
                _touchCutObj.SetActive(false);*/
            }
            return _touchCutObj;
        }
    }


    bool _isInited = false;
    /*public override void Init()
    {
        if (_isInited)
            return;

        base.Init();
		missRecv = false;
        GameObject.DontDestroyOnLoad(gameObject);
        //NetworkReachability _networkMode;
        //_networkMode = Application.internetReachability;
        _isInited = true;
        _netWaitCiteCount = 0;
    }*/

    void Update()
    {
		if (SocketManager.Instance != null) {
			connect = !SocketManager.GetInstance ().IsDisConnect;
		}
        _CheckWaitTime(Time.deltaTime);
    }

    /// <summary>
    /// 重登
    /// </summary>
    void _ReLogon()
    {
        _isInited = false;
		_netWaitCiteCount = 0;

		SocketManager.GetInstance ().Clear ();
		/*BaseModel.instance.Clear ();
        BaseControl.Instance.LoadScene(SceneType.Logon);*/
    }

    /// <summary>
    /// 重连
    /// </summary>
    void _ReConnect()
    {
        SocketManager.Instance.Reconnect();
    }

    #region 网络等待
    int _netWaitCiteCount;
    float _waitTimeCount;
    float _waitShowTimeCount;
    /*NetWaitDialog _waitDialog;
    NetWarningDialog _warningDialog;*/
    public void AddWait(NetCommand command)
    {
        /*if (_netWaitCiteCount == 0 && _waitDialog == null)
            _waitShowTimeCount = 0;
        _netWaitCiteCount++;

        touchCutObj.SetActive(true);

		EditorDebug.LogWarning("CurrentWaitMessageCount + 1 = " , _netWaitCiteCount , "    " , command.ToString());
    */
    }

    public void CutWait(NetCommand command)
    {
        /*_waitTimeCount = 0;
        _netWaitCiteCount--;
        if (_netWaitCiteCount <= 0)
            _RemoveWaitDialog();

		EditorDebug.LogWarning("CurrentWaitMessageCount - 1 = " , _netWaitCiteCount , "    " , command.ToString());
        */
    }

    void _CheckWaitTime(float dt)
    {
        /*if (_warningDialog != null)
            return;

        if (_waitDialog == null)
        {
            if (_netWaitCiteCount > 0)
            {
                _waitShowTimeCount += dt;
                if (_waitShowTimeCount > 1)
                    _ShowWaitDialog();
            }
        }
        else
        {
            _waitTimeCount += dt;
			if (_waitTimeCount > 10) {
				HomeNet.SendLogOut ();
				_ShowWarningDialog ();
			}
        }*/
    }

    void _ShowWaitDialog()
    {
        //_waitDialog = NetWaitDialog.Open();
        _waitTimeCount = 0;
    }
    void _RemoveWaitDialog()
    {
        touchCutObj.SetActive(false);
        /*if (_waitDialog != null)
        {
            ObjManager.Instance.Despawn(_waitDialog.gameObject);
            _waitDialog = null;
        }*/
    }
    void _ShowWarningDialog()
    {
        /*if (NetWarningDialog.isOpening || uiTrm == null)
            return;

        if (SocketManager.Instance.IsDisConnect)
            _warningDialog = NetWarningDialog.Open("重连", _ReConnect, uiTrm);
        else
            _warningDialog = NetWarningDialog.Open("重登", _ReLogon, uiTrm);

        _RemoveWaitDialog();*/
    }

    /// <summary>
    /// 0重连  1重登
    /// </summary>
    /// <param name="type"></param>
    public void ShowWarningDialog(int type)
    {
        /*if (NetWarningDialog.isOpening)
            return;

        if(type == 0)
            _warningDialog = NetWarningDialog.Open("重连", _ReConnect, uiTrm);
        else if(type == 1)
            _warningDialog = NetWarningDialog.Open("重登", _ReLogon, uiTrm);
        _RemoveWaitDialog();*/
    }

    void _RemoveWarningDialog()
    {
        /*touchCutObj.SetActive(false);
        _netWaitCiteCount = 0;
        if(_warningDialog != null)
            _warningDialog = null;*/
    }
    #endregion

}
