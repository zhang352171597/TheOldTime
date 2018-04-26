using UnityEngine;
using System.Collections;

/// <summary>
/// 按时删除
/// </summary>
public class SkillTweenFinishByTime : SkillTweenBase 
{
    /// <summary>
    /// 持续时间
    /// </summary>
    public float duration;

    float timeCount;

    public override void Play()
    {
        base.Play();
        timeCount = 0;
    }

    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        timeCount += dt;
        if (timeCount >= duration)
            _FinishRoot();
    }
}
