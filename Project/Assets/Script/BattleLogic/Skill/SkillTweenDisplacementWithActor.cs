using UnityEngine;
using System.Collections;

public class SkillTweenDisplacementWithActor : SkillTweenBase 
{
    /// <summary>
    /// 位移距离
    /// </summary>
    public float distance;
    /// <summary>
    /// 位移时间
    /// </summary>
    public float duration;
    Vector3 _currentVelocity;
    Vector3 _direction;
    Vector3 _offsetWithAttacker;
    float _speed;
    public override void Play()
    {
        base.Play();
        _direction = attacker.forward;
        _speed = distance / duration;
        _offsetWithAttacker = attacker.position - position;
        UIMgr.Instance.ChangeInputState(false);
        AddTimerEvent(_End, duration);
    }
    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        DisplacementUpdate(dt);
    }
    void DisplacementUpdate(float dt)
    {
        var lastPos = rootSkill.data.attacker.position;
        var targetPos = attacker.position + _direction * _speed * dt;
        attacker.position = Vector3.SmoothDamp(attacker.position, targetPos, ref _currentVelocity, 0.01f);
        position = attacker.position - _offsetWithAttacker;
    }
    void _End()
    {
        _FinishRoot();
        UIMgr.Instance.ChangeInputState(true);
    }
}
