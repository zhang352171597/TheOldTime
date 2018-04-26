using UnityEngine;
using System.Collections;

public class SkillTweenCreater : SkillTweenBase
{
    public float delayTime;
    public enSkillTweenCreateType type;
    public GameObject targetPrefab;
    public float duration;
    public GameObject[] hindEffects;
    GameObject _effect;


    public override void Play()
    {
        base.Play();
        if (type == enSkillTweenCreateType.onStart)
            AddTimerEvent(_CreateClip, delayTime);
    }
    protected override void TryFinishClip()
    {
        base.TryFinishClip();
        if (type == enSkillTweenCreateType.onFinish)
            AddTimerEvent(_CreateClip, delayTime);
    }
    void _CreateClip()
    {
        _effect = ObjManager.Instance.addChild(targetPrefab, transform);
        AddTimerEvent(_DespawnClip, duration);
        _HindCheck(false);
    }
    void _DespawnClip()
    {
        _HindCheck(true);
        ObjManager.Instance.Despawn(_effect);
        _FinishRoot();
    }
    void _HindCheck(bool state)
    {
        for(int i = 0 ; i < hindEffects.Length; ++i)
        {
            hindEffects[i].gameObject.SetActive(state);
        }
    }
}
