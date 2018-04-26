using UnityEngine;
using System.Collections;

/// <summary>
/// 频率触发控制
/// </summary>
public class SkillTweenWithFrequency : SkillTweenBase
{
    public float frequency;
    /// <summary>
    /// 生效次数
    /// </summary>
    public int triggerTimes;

    /// <summary>
    /// 频率计时器
    /// </summary>
    float _frequencyCount;
    int _triggerCount;

    public override void Play()
    {
        base.Play();
        _triggerCount = 0;
    }

    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        _CheackFrequency(dt);
    }

    void _CheackFrequency(float dt)
    {
        if (frequency <= 0)
            return;

        _frequencyCount += dt;
        if (_frequencyCount >= frequency)
        {
            rootSkill.AddBuff();
            _frequencyCount = 0;
            _triggerCount++;
            if (_triggerCount == triggerTimes)
                _FinishRoot();
        }
    }

}
