using UnityEngine;
using System.Collections;


public class SkillTweenBase : MonoTimer
{
    [Header("结束即结束技能")]
    public bool isMainTween;

    SkillTweenBase[] _otherTweens;
    protected SkillTweenBase[] otherTweens
    {
        get
        {
            if (_otherTweens == null)
                _otherTweens = GetComponents<SkillTweenBase>();
            return _otherTweens;
        }
    }
    Vector3 _position;
    public Vector3 position
    {
        get { return _position; }
        set 
        { 
            _position = value;
            _UpdatePos();
        }
    }
    Vector3 _forward = Vector3.forward;
    public Vector3 forward
    {
        get { return _forward; }
        set { _forward = value; }
    }

    SingleSkillBase _rootSkill;
    protected SingleSkillBase rootSkill
    {
        get
        {
            if (_rootSkill == null)
                _rootSkill = GetComponentInParent<SingleSkillBase>();
            return _rootSkill;
        }
    }
    protected ActorBase attacker
    {
        get { return rootSkill.data.attacker; }
    }
    /// <summary>
    /// 优先级 --技能初始化的时候通过优先级进行排序 在逻辑执行时 优先级越大越先被执行
    /// </summary>
    public virtual int Priority
    {
        get { return 0; }
    }
    public virtual void Play()
    {
        TimerBegin();
        position = rootSkill.position;
        _UpdatePos();
    }

    public virtual void UpdateLogic(float dt)
    {
        TimerUpdate(dt);
    }

    public virtual void OnCollided(Transform colliderTrm)
    {

    }

    public virtual void OnFinish()
    {

    }
    protected virtual void TryFinishClip()
    {

    }
    /// <summary>
    /// 尝试结束技能
    /// </summary>
    /// <param name="force"></param>
    protected void _FinishRoot()
    {
        ///如果：强制或主技能片段则直接关闭/否则通知其它剪辑
        if (isMainTween)
            rootSkill.Finish();
        else
        {
            for(int i = 0 ; i < otherTweens.Length; ++i)
            {
                otherTweens[i].TryFinishClip();
            }
        }
    }
    /// <summary>
    /// 强制结束技能
    /// </summary>
    /// <param name="force"></param>
    protected void _FinishRootForce()
    {
        rootSkill.Finish();
    }
    void _UpdatePos()
    {
        transform.position = position;
        var q = Quaternion.LookRotation(forward, Vector3.up);
        transform.rotation = q;
    }

}
