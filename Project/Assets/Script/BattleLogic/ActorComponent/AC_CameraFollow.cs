using UnityEngine;
using System.Collections;
/// <summary>
/// 相机跟随控制器
/// </summary>

public class AC_CameraFollow : ActorComponentBase
{
    Vector3 DefaultLOS = new Vector3(0, 0, -10f);
    Vector3 DefaultRot = new Vector3(0, 0, 0);

    Transform _cameraRoot;
    Transform _losTrm;
    Transform _rotTrm;
    Animator _ani;

    Vector3 _currentLOS;
    Vector3 _currentRot;
    Vector3 _currentCenterPos;
    Vector3 _targetCenterPos;
    Vector3 _currentVelocity;

    public override void OnAwake()
    {
        base.OnAwake();
        actor.Bind(enActorMessageType.onUpdatePos, _OnPositionChange);
        actor.Bind(enActorMessageType.tryShakeCamera, _TryShakeCamera);
    }
    public override void OnStart()
    {
        base.OnStart();
        _cameraRoot = GameObject.FindGameObjectWithTag("MainCamera").transform;
        _losTrm = _cameraRoot.GetChild(0);
        _rotTrm = _losTrm.GetChild(0);
        _ani = _rotTrm.GetComponentInChildren<Animator>();
        _currentLOS = DefaultLOS;
        _currentRot = DefaultRot;
        _targetCenterPos = actor.position;
        _currentCenterPos = _targetCenterPos;
        _cameraRoot.localPosition = _currentCenterPos;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        actor.UnBind(enActorMessageType.onUpdatePos, _OnPositionChange);
        actor.UnBind(enActorMessageType.tryShakeCamera, _TryShakeCamera);
    }

    public override void LogicUpdate(float dt)
    {
        base.LogicUpdate(dt);
        if (_cameraRoot == null || _currentCenterPos == _targetCenterPos)
            return;

        _currentCenterPos = Vector3.SmoothDamp(_currentCenterPos, _targetCenterPos, ref _currentVelocity, 0.01f);
        _cameraRoot.localPosition = _currentCenterPos;
        //主相机位置变更过程需要通知消息中心 涉及到一些角色头标位置更新
        MessageCenter.Instance.Execute(enGameMessageType.worldCameraUpdate);
    }

    #region Callback
    void _OnPositionChange(params System.Object[] objs)
    {
        _targetCenterPos = actor.position;
        if (_targetCenterPos.y > 4.5f)
            _targetCenterPos.y = 4.5f;
    }
    void _TryShakeCamera(params System.Object[] objs)
    {
        var shakeLevel = (int)objs[0];
        _ani.SetTrigger("shake" + shakeLevel);
    }
    #endregion

}
