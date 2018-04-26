using UnityEngine;
using System.Collections;

public class SkillTweenFlyWithActor : SkillTweenBase
{
    /// <summary>
    /// 跳跃高度
    /// </summary>
    public float jumpHeight;
    /// <summary>
    /// 攻击检测距离
    /// </summary>
    public float maxAttackDis;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public float attackDis;
    /// <summary>
    /// 速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 下落加速度
    /// </summary>
    public float fallAddition;
    /// <summary>
    /// 终点距离目标偏移
    /// </summary>
    public float offsetDis;

    float _currentSpeed;
    Vector3 _currentVelocity;
    Vector3 _targetPos;
    #region FSMAbout
    FSM _fsm = new FSM();
    FSMState _idle = new FSMState("idle");
    FSMState _jump = new FSMState("jump");
    FSMState _fall = new FSMState("fall");
    void Awake()
    {
        _fsm.AddState(_idle, null, null, null);
        _fsm.AddState(_jump, _JumpEnter, _JumpUpdate, null);
        _fsm.AddState(_fall, _FallEnter, _FallUpdate, null);
    }
    void _JumpEnter()
    {
        _currentVelocity = Vector3.zero;
    }
    void _JumpUpdate(float dt)
    {
        var nextPos = position + Vector3.up * _currentSpeed * dt;
        if (nextPos.y < jumpHeight)
        {
            position = nextPos;
            attacker.position = Vector3.SmoothDamp(attacker.position, position, ref _currentVelocity, 0.01f);
        }
        else
            _fsm.SetState(_fall);
    }
    void _FallEnter()
    {
        _currentVelocity = Vector3.zero;
    }
    void _FallUpdate(float dt)
    {
        _currentSpeed += fallAddition * dt;
        var dir = (_targetPos - position).normalized;
        var tempDis = _currentSpeed * dt;
        var nextPos = position + dir * tempDis;
        if (Vector3.Distance(position, _targetPos) <= tempDis || nextPos == position)
        {
            position = _targetPos;
            attacker.position = position;
            AddTimerEvent(() =>
            {
				_End();
            } , 0.2f);
			_fsm.SetState(_idle);
        }
        else
        {
            position = nextPos;
            attacker.position = Vector3.SmoothDamp(attacker.position, position, ref _currentVelocity, 0.01f);
        }
    }
    #endregion
    public override void Play()
    {
        base.Play();
		UIMgr.Instance.ChangeInputState(false);
        position = attacker.position;
        _currentSpeed = speed;
        RaycastHit hitinfo;
        if (Physics.Raycast(position, attacker.forward, out hitinfo, maxAttackDis))
        {
            _targetPos = new Vector3(hitinfo.point.x, 0, hitinfo.point.z);
            var dir = (_targetPos - position).normalized;
            _targetPos += dir * offsetDis;
        }
        else
            _targetPos = position + attacker.forward * attackDis;
        _fsm.SetState(_jump);
    }
    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        _fsm.Update(dt);
    }
    void _End()
    {
		attacker.Execute(enActorMessageType.tryShakeCamera, 1);
        _FinishRoot();
        UIMgr.Instance.ChangeInputState(true);
    }
}
