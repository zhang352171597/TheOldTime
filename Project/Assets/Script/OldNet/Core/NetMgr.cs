using UnityEngine;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;
//using LitJson;
using JSON;
using System.Threading;

public enum NetCommand
{
    ELoginUser = 99,
}


public class BufferObject
{
    public Socket _tcpSock = null;
    public int BufferSize;
    public byte[] buffer;
    public NetCommand commandId;
    public void CreateBuffer()
    {
        buffer = new byte[BufferSize];
    }
    public StringBuilder Data = new StringBuilder();
}
public class NetMgr
{

    //包头长度
    private const int RecviceHeadLength = 21;
    //是否接收数据
    private static bool m_bIsReceive = false;
    //当前接收大小
    private static int m_nReceiveLength = 0;
    //当前创建的socket
    private static Socket _tcpSock = null;
    // 超时事件
    private static ManualResetEvent timeoutobject = new ManualResetEvent(false);
    //是否连接
    //private static bool m_bIsConnect = false;
    //Socket错误信息
    private static Exception m_socketexception;
    private static int m_timeOutmsec = 30 * 1000;

	public static string m_hostJY = "192.168.0.175";
	public static string m_hostYUN = "180.150.177.134";
	public static string m_hostIntranet = "192.168.0.129";
	public static string m_hostOutnet = "180.150.177.134";
    public static string m_hostLY = "192.168.0.176";
#if UNITY_EDITOR
//	public static string m_host = "180.150.177.134";
    public static string m_host = "192.168.0.175";  //Jy
//	public static string m_host = "180.150.177.134"; // 云
//	public static string m_host = "192.168.0.25"; // 内网
//	public static string m_host = "192.168.0.129"; 
#else
	    public static string m_host = "180.150.177.134";
//	public static string m_host = "192.168.0.25";
#endif

    /*
     现在天神有几台服务器：
    内网 192.168.0.25
    建勇 192.168.0.175
    云  180.150.177.134
    原本的192.168.0.129 给端游做测试了。*/

	public event System.Action onDisConnect;

    public static int m_port = 16000;

    //_socket = new SocketClient("192.168.0.175", 13000, 1, 1, NULL); // 跑酷 剑勇
    //_socket = new SocketClient("180.150.177.134", 13000, 1, 1, NULL); // 跑酷 云

    public static NetMgr Instance;
    public static NetMgr GetInstance()
    {
        if (Instance == null) {
            Instance = new NetMgr();
        }
        return Instance;
    }

    public static void Destroy()
    {
        Instance = null;
    }

    public bool Connect()
    {
		Debug.Log ("Host IP " + NetMgr.m_host);

        _tcpSock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ip = IPAddress.Parse(m_host);
        IPEndPoint ipe = new IPEndPoint(ip, m_port);
        timeoutobject.Reset();
        IAsyncResult ResultType = _tcpSock.BeginConnect(ipe, new AsyncCallback(CallbackConnect), _tcpSock);
		Debug.Log ("======>" + m_host.ToString ());
        if (timeoutobject.WaitOne(m_timeOutmsec, false)) {
            if (SocketManager.m_bIsConnect)
            {
                Receive(_tcpSock);
                return true;
            } else {
//                JsonData jData = new JsonData();
//                jData["Data"] = "连接失败，请检查网络设置";
//                GameManager.GetInstance().AddGameState(GameState.EGameStateDialogNet, jData);
				throw new ArgumentException("连接失败，请检查网络设置");
            }
        } else {
            _tcpSock.Close();
//            JsonData jData = new JsonData();
//            jData["Data"] = "连接失败，请检查网络设置";
//            GameManager.GetInstance().AddGameState(GameState.EGameStateDialogNet, jData);
			throw new ArgumentException("连接失败，请检查网络设置");
        }
        return false;
        //connectDone.WaitOne ();
        //sendDone.WaitOne ();

    }
	void OnDisConnect(){
		if(onDisConnect != null)
			onDisConnect();
		onDisConnect = null;
	}

    public bool Reconnect()
    {
        //关闭socket
        //_tcpSock.Shutdown(SocketShutdown.Both);
        //_tcpSock.Disconnect(true);

        SocketManager.m_bIsConnect = false;
        //_tcpSock.Close();

        //创建socket
        return Connect();
    }

    /// <summary>
    /// 断网--测试
    /// </summary>
    public void CuttingConnet()
    {
        SocketManager.m_bIsConnect = false;
//		_tcpSock.Shutdown(SocketShutdown.Both);
//       	_tcpSock.Disconnect(false);
        _tcpSock.Close();
        _tcpSock = null;
    }

    private void CallbackConnect(IAsyncResult ar)
    {
        try {
            SocketManager.m_bIsConnect = false;
            Socket socket = (Socket)ar.AsyncState;

            if (socket != null) {
                socket.EndConnect(ar);
                //timeoutobject.Set();
                SocketManager.m_bIsConnect = true;
            }
        } catch (Exception ex) {
            SocketManager.m_bIsConnect = false;
            m_socketexception = ex;
        } finally {
            timeoutobject.Set();
        }
    }
    public void Receive(Socket _tcpSock)
    {
        if (_tcpSock == null || !_tcpSock.Connected)
            return;
        BufferObject ReceiveBuffer = new BufferObject();
        ReceiveBuffer._tcpSock = _tcpSock;
        ReceiveBuffer.BufferSize = RecviceHeadLength;
        ReceiveBuffer.CreateBuffer();
        m_nReceiveLength = ReceiveBuffer.BufferSize;
        m_bIsReceive = false;
        _tcpSock.BeginReceive(ReceiveBuffer.buffer, 0, ReceiveBuffer.BufferSize, 0, new AsyncCallback(ReceiveCallback), ReceiveBuffer);
    }
    public void ReceiveCallback(IAsyncResult ar)
    {
        try {
            BufferObject ReceiveBuffer = (BufferObject)ar.AsyncState;
            Socket m_Socket = ReceiveBuffer._tcpSock;
            if (m_Socket == null  || !m_Socket.Connected)
                return;
            int bytesRead = ReceiveBuffer._tcpSock.EndReceive(ar);
            //如果没有接收完毕则继续接收
            if (m_nReceiveLength > 0) {
                ReceiveBuffer.Data.Append(Encoding.UTF8.GetString(ReceiveBuffer.buffer, 0, bytesRead));
                m_nReceiveLength -= bytesRead;
                if (m_nReceiveLength <= 0) {
                    if (m_bIsReceive) {
                        //Debug.LogError("ReceiveBuffer.Data.Length: " + ReceiveBuffer.Data.Length);//Debug.LogError("ReceiveBuffer.BufferSize: " + ReceiveBuffer.BufferSize);//Debug.LogError("(ReceiveBuffer.buffer.ToString()" + System.Text.Encoding.UTF8.GetString(ReceiveBuffer.buffer));//Debug.LogError("(ReceiveBuffer.Data.ToString()" + ReceiveBuffer.Data);//Debug.LogError("resurlst" + System.Text.Encoding.UTF8.GetString(Compression.deCompressBytes(ReceiveBuffer.buffer)));
                        string receiveDate = Encoding.UTF8.GetString(Compression.deCompressBytes(ReceiveBuffer.buffer));
                        JSONNode jsondata = JsonRead.Parse(receiveDate);
//                        JsonReader jsonR = new JsonReader(receiveDate);
//                        JsonData jData = JsonMapper.ToObject(jsonR);
//                        SocketManager.GetInstance().RecvMessage(ReceiveBuffer.commandId, jData);
						SocketManager.GetInstance().RecvMessage(ReceiveBuffer.commandId, jsondata);
                        Receive(m_Socket);
                        return;

                        //未压缩的
                        //if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize) {
                        //    JsonReader jsonR = new JsonReader(ReceiveBuffer.Data.ToString());
                        //    JsonData jData = JsonMapper.ToObject(jsonR);
                        //    SocketManager.GetInstance().RecvMessage(ReceiveBuffer.commandId, jData);
                        //    Receive(m_Socket);
                        //    return;
                        //}
                    }
                    else {
                        if (ReceiveBuffer.Data.Length == ReceiveBuffer.BufferSize) {
                            NetCommand commandId;
                            int size;
                            int zipLenght;
                            unPackHead(ReceiveBuffer.buffer, out commandId, out size, out zipLenght);
                            m_bIsReceive = true;
                            BufferObject RecvDataBuffer = new BufferObject();
                            RecvDataBuffer._tcpSock = ReceiveBuffer._tcpSock;
                            //RecvDataBuffer.BufferSize = size - 4;
                            RecvDataBuffer.BufferSize = size-8;
                            RecvDataBuffer.commandId = commandId;
                            m_nReceiveLength = RecvDataBuffer.BufferSize;
                            RecvDataBuffer.CreateBuffer();
                            _tcpSock.BeginReceive(RecvDataBuffer.buffer, 0, RecvDataBuffer.BufferSize, 0, new AsyncCallback(ReceiveCallback), RecvDataBuffer);
                            return;
                        }
                    }
                }
				_tcpSock.BeginReceive(ReceiveBuffer.buffer, bytesRead, m_nReceiveLength, 0, new AsyncCallback(ReceiveCallback), ReceiveBuffer);
            } else {
                Debug.LogWarning("Net Manager m_nReceiveLength <= 0");
            }
        } catch (Exception ex) {
            Debug.LogError(ex.Message);
			CuttingConnet ();
        }
    }

    private void unPackHead(byte[] head, out NetCommand commandId, out int size, out int zipLenght)
    {
        int head0 = head[0];
        int head1 = head[1];
        int head2 = head[2];
        int head3 = head[3];
        int protoVersion = head[4];
        int serverVersion = System.BitConverter.ToInt32(head, 5);
        // todo check the head, version
        System.Array.Reverse(head, 9, 4);
        size = System.BitConverter.ToInt32(head, 9);

        System.Array.Reverse(head, 13, 4);
        commandId = (NetCommand)System.BitConverter.ToInt32(head, 13);

        System.Array.Reverse(head, 17, 4);
        zipLenght = System.BitConverter.ToInt32(head, 17);

//        if (commandId != NetCommand.TestPing)
//            Debug.Log("Receive size: " + size + " zip lenght: " + zipLenght);
    }

    private byte[] packMsg(byte[] msg, NetCommand commandId)
    {
        byte[] head = new byte[17];
        head[0] = 20;
        head[1] = 15;
        head[2] = 8;
        head[3] = 18;
        head[4] = 0; // protoVersion

        Array.Copy(System.BitConverter.GetBytes(0), 0, head, 5, 4); // serverVersion

        byte[] lengthByte = new byte[4];
        lengthByte = System.BitConverter.GetBytes(msg.Length + 4);
        Array.Reverse(lengthByte);
        Array.Copy(lengthByte, 0, head, 9, 4);

        byte[] commandIdByte = new byte[4];
        commandIdByte = System.BitConverter.GetBytes((int)commandId);
        Array.Reverse(commandIdByte);
        Array.Copy(commandIdByte, 0, head, 13, 4);

        byte[] ret = new byte[head.Length + msg.Length];
        Array.Copy(head, 0, ret, 0, head.Length);
        Array.Copy(msg, 0, ret, head.Length, msg.Length);
        return ret;
    }
	public bool IsDisConnect{
		get{ 
			return _tcpSock == null || _tcpSock.Connected == false;
		}
	}
    public void SendMsg(byte[] msg, NetCommand commandId)
    {
        byte[] data = packMsg(msg, commandId);

        if (_tcpSock == null || _tcpSock.Connected == false) {
//            JsonData jData = new JsonData();
//            jData["Data"] = "参数socket为null，或者未连接到远程计算机";
//            GameManager.GetInstance().AddGameState(GameState.EGameStateDialogNet, jData);
            throw new ArgumentException("参数socket为null，或者未连接到远程计算机");
        } else if (data == null || data.Length == 0) {
//            JsonData jData = new JsonData();
//            jData["Data"] = "参数data为null ,或者长度为 0";
//            GameManager.GetInstance().AddGameState(GameState.EGameStateDialogNet, jData);
            throw new ArgumentException("参数data为null ,或者长度为 0");
        } else {

            int flag = 0;
            try {
                int left = data.Length;
                int sndLen = 0;
                int hasSend = 0;

                while (true) {
                    if ((_tcpSock.Poll(m_timeOutmsec, SelectMode.SelectWrite) == true)) {
                        if (IsDisConnect)
                            return;
                        //收集了足够多的传出数据后开始发送  
                        sndLen = _tcpSock.Send(data, hasSend, left, SocketFlags.None);
                        left -= sndLen;
                        hasSend += sndLen;

                        //数据已经全部发送  
                        if (left == 0) {
                            flag = 0;
                            break;
                        } else {
                            //数据部分已经被发送  
                            if (sndLen > 0) {
                                continue;
                            } else { //发送数据发生错误                                                  
                                flag = -2;
                                break;
                            }
                        }
                    } else { //超时退出                                                          
                        flag = -1;
                        break;
                    }
                }
            } catch (SocketException) {
                //Log  
                flag = -3;
            }
            if (flag != 0)
                Debug.Log("send flag : " + flag);
        }
    }
}