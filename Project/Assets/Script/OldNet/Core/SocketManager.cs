//using LitJson;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSON;



public enum GameNetState
{
	CloseSendLoad,
}

public class NetSendObj
{
	public byte[] msg;
	public NetCommand command;
	public Action<JSONNode> func;

	public NetSendObj (byte[] _msg, NetCommand _commandId, Action<JSONNode> _func)
	{
		msg = _msg;
		command = _commandId;
		func = _func;
	}
}

public class SocketManager
{
	private Dictionary<NetCommand, Action<JSONNode>> _FuncList;
	public NetMgr _NetMgr;
	private NetSendObj bak;
	public static bool m_bIsConnect = false;
	const string SocketKey = "yinghuang";
	public Action<NetCommand,JSONNode> ReceiveData;

	public event System.Action<NetCommand,JSONNode> HandlePushMessage;
	public event System.Action OnDisConnect;
	public System.Action<NetCommand,JSONNode> errorCallback;
	public SocketManager ()
	{
		_FuncList = new Dictionary<NetCommand, Action<JSONNode>> ();
	}

	public void Connect ()
	{
		_NetMgr = new NetMgr ();
		_NetMgr.Connect ();
	}

	public static SocketManager Instance;

	public static SocketManager GetInstance ()
	{
		if (Instance == null) {
			Instance = new SocketManager ();
		}
		return Instance;
	}

	public static void Destroy ()
	{
		Instance = null;
	}
	public void Clear(){
		_FuncList = new Dictionary<NetCommand, Action<JSONNode>> ();
	}

	public void RecvMessage (NetCommand commandId, JSONNode jData)
	{
		bool isFunc = _FuncList.ContainsKey (commandId);
		if (jData ["State"].AsInt == 1) {   
			if (isFunc) {
				Loom.QueueOnMainThread (() => {
					if (_FuncList.ContainsKey (commandId)) {
                        //NetCheckMgr.Instance.CutWait(commandId);
						if (jData ["Data"].ToString ().Length > 1)
						if (ReceiveData != null)
							ReceiveData (commandId, jData ["Data"]);
                        ///解析回传
						//BaseModel.Instance.ReceiveData (commandId, jData ["Data"]);
						var tempFuc = _FuncList [commandId];
						_FuncList.Remove (commandId);
						tempFuc (jData);
					} else {
						Debug.LogError ("Not Contains Key :" + commandId);
					}

				});
			} else {
				PushMessage (commandId, jData);
			}
		} else {    //服务器错误回传
            Loom.QueueOnMainThread(() =>
            {
                //BaseModel.Instance.ReceiveErrorData(commandId, jData);
                if (_FuncList.ContainsKey(commandId))
                {
                    //NetCheckMgr.Instance.CutWait(commandId);
                    Debug.Log("ErrorData ... " + jData["Data"]);
                    if (isFunc)
                    {
                        _FuncList.Remove(commandId);
                    }
                    if (errorCallback != null)
                    {
                        errorCallback(commandId, jData);
                        errorCallback = null;
                    }
                }
            });
		}
	}


	//private
	public void SendByteMessage (byte[] jData, NetCommand commandId, Action<JSONNode> func)
	{
		byte[] msg = XXTea.Encrypt2 (jData, SocketKey);
		SendMessage (msg, commandId, func);
	}

	public void SendMessage (ListJsonData jData, NetCommand commandId, Action<JSONNode> func)
	{
		byte[] msg = XXTea.Encrypt2 (jData.GetString (), SocketKey);
		var size = sizeof(byte) * msg.Length;
		SendMessage (msg, commandId, func);
	}

	public void testSend (byte[] msg, NetCommand commandId, Action<JSONNode> func)
	{
		SendMessage (msg, commandId, func);
	}

	void SendMessage (byte[] msg, NetCommand commandId, Action<JSONNode> func)
	{
        //NetCheckMgr.Instance.AddWait(commandId);
		if (!m_bIsConnect)
			return;

		if (bak == null && commandId != NetCommand.ELoginUser) {
			bak = new NetSendObj (msg, commandId, func);
		}
		bool isFunc = _FuncList.ContainsKey (commandId);
		if (!isFunc && func != null)
			_FuncList.Add (commandId, func);
		else {
			Debug.Log ("发送协议失败，协议池内已经存在["+commandId.ToString()+"],服务器未返还消息");
			return;
		}

		if (IsDisConnect) {
			if (OnDisConnect != null)
				OnDisConnect ();
			return;
		}
		_NetMgr.SendMsg (msg, commandId);
	}



	void PushMessage (NetCommand commandId, JSONNode jData)
	{
		Loom.QueueOnMainThread (() => {
			HandlePushMessage (commandId, jData);
            //解析推送
			//NetBattle.NetPushHelper.Instance.PushMessage (commandId, jData);
		});
	}

	public void ReSend ()
	{
		if (bak != null && _FuncList.ContainsKey (bak.command)) {
			//Debug.LogError("RE SendMessage id :" + bak.command + " data :" + System.Text.Encoding.Default.GetString(bak.msg));
			_NetMgr.SendMsg (bak.msg, bak.command);
		}
	}

	public bool IsDisConnect {
		get 
        { 
			if (_NetMgr == null)
				return true;
			return _NetMgr.IsDisConnect; 
		}
	}

    public void CuttingConnet()
    {
        if (_NetMgr == null)
            Debug.LogError("NetMgr is Not Exist");
        else
            _NetMgr.CuttingConnet();
    }

	public void Reconnect ()
	{
		if (_NetMgr == null) {
//			sendReconnect ();
			return;
		}

		if (_NetMgr.Reconnect ()) {
			sendReconnect ();

		}
	}
	void sendReconnect(){
		//Stage.MessageCenterEx.Instance.Execute (enNetAction.OnReconnect);
		_FuncList.Clear ();
		ListJsonData list = ListJsonData.Init ();
		//list.Add ("UUID", BaseModel.Instance.UUID);
		list.Add ("Simple", 1);
		list.Add ("Repeat", 1);
		SendMessage (list, NetCommand.ELoginUser, delegate(JSONNode data) {
			ReSend ();
		});
	}
}
