using UnityEngine;
using System.Collections;

/// <summary>
/// 非指向位移控制器
/// </summary>
public class SkillTweenWithDir : SkillTweenBase 
{
    /// <summary>
    /// 位移速度
    /// </summary>
    public float speed;

    /// <summary>
    /// 最大位移距离
    /// </summary>
    public float maxDis = 0;

    /// <summary>
    /// 命中即删除技能
    /// </summary>
    public bool finishAfterHited;

    /// <summary>
    /// 出生点
    /// </summary>
    Vector3 _bornPos;

    public override void Play()
    {
        base.Play();
        _bornPos = rootSkill.data.attacker.position;
        position = _bornPos;
        forward = rootSkill.data.attacker.forward;
    }

    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        position += forward * speed * dt;
        _CheackMaxDis();
    }

    public override void OnCollided(Transform colliderTrm)
    {
        base.OnCollided(colliderTrm);
        if(finishAfterHited)
            _FinishRoot();
    }

    void _CheackMaxDis()
    {
        if (maxDis == 0)
            return;

        var currentDis = Vector3.Distance(_bornPos, position);
        if (currentDis >= maxDis)
            _FinishRoot();
    }

}
