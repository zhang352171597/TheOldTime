using UnityEngine;
using System.Collections;

public class SkillTweenFollow : SkillTweenBase
{
	ActorBase _followTarget;
	Vector3 _currentVelocity;
    public override void Play()
    {
		_followTarget = rootSkill.data.attacker;
        base.Play();
    }
    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
		_MoveUpdate (dt);
    }
    void _MoveUpdate(float dt)
    {
        if (_followTarget != null)
        {
            position = Vector3.SmoothDamp(position, _followTarget.position + _followTarget.bodyOffset, ref _currentVelocity, 0.01f);
        }
        else
            _FinishRoot();
    }
}
