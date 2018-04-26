using UnityEngine;
using System.Collections;

public class SingleFunctionSkill : SingleSkillBase
{
    /// <summary>
    /// 技能表现控制器
    /// </summary>
    SkillTweenBase[] tweens;

    void Awake()
    {
        tweens = GetComponentsInChildren<SkillTweenBase>();
    }

    public override void Play(enSkillConstructionData data)
    {
        base.Play(data);
        for (int i = 0; i < tweens.Length; ++i)
        {
            tweens[i].Play();
            tweens[i].transform.position = position;
        }
    }

    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        for (int i = 0; i < tweens.Length; ++i)
        {
            tweens[i].UpdateLogic(dt);
        }
    }
}
