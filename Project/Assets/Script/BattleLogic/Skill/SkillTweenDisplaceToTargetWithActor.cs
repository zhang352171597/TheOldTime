using UnityEngine;
using System.Collections;

public class SkillTweenDisplaceToTargetWithActor : SkillTweenBase
{
    /// <summary>
    /// 目标点距离目标角色
    /// </summary>
    public float minDistance;
    /// <summary>
    /// 速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 加速度
    /// </summary>
    public float speedAddition;
    Vector3 _currentVelocity;
    float _currentSpeed;
    ActorBase _target;
    bool _isActive;
    public override void Play()
    {
        base.Play();
        _target = rootSkill.data.belongStateData.releaseData.lastHittedActor;
        attacker.forward = (_target.position - attacker.position).normalized;
        _isActive = true;
        _currentSpeed = speed;
        position = attacker.position;
        UIMgr.Instance.ChangeInputState(false);
    }
    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        if (_isActive)
            DisplacementUpdate(dt);
    }
    void DisplacementUpdate(float dt)
    {
        if (_target == null)
            _FinishRootForce();
        else
        {
            _currentSpeed += speedAddition * dt;
            var dir = (_target.position - attacker.position).normalized;
            var tempDis = _currentSpeed * dt;
            var nextPos = position + dir * tempDis;
            tempDis = minDistance > 0 ? minDistance : tempDis;
            if (Vector3.Distance(position, _target.position) <= tempDis || nextPos == position)
            {
                _isActive = false;
                attacker.position = position;
                position = _target.position;
                _End();
            }
            else
            {
                position = nextPos;
                var targetPos = Vector3.SmoothDamp(attacker.position, position, ref _currentVelocity, 0.01f);
                targetPos.y = attacker.position.y;
                attacker.position = targetPos;
            }
        }
    }
    void _End()
    {
        _FinishRoot();
        UIMgr.Instance.ChangeInputState(true);
    }
}
