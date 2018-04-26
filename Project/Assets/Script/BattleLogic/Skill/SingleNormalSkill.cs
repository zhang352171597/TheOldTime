using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SingleNormalSkill : SingleSkillBase
{
    /// <summary>
    /// 为目标添加buff
    /// </summary>
    public string[] buffId;
    /// <summary>
    /// 命中相同阵营
    /// </summary>
    public bool collidedWithSameCamp;
    /// <summary>
    /// 命中瞬间是否施加buff
    /// </summary>
    public bool isHitOnTrigger;
    /// <summary>
    /// 碰撞延迟有效
    /// </summary>
    public float colliderDelay;
    /// <summary>
    /// 碰撞器持续时间
    /// </summary>
    public float colliderLife;
    /// <summary>
    /// 单次最大命中数
    /// </summary>
    public int maxHitByOnce;
    /// <summary>
    /// 允许多次命中同一目标
    /// </summary>
    public bool repeatAttackSameTarget;
    /// <summary>
    /// 技能表现的行为反馈于技能数据
    /// </summary>
    public enSkillActionBackType actionBackType;
    /// <summary>
    /// 已命中的目标
    /// </summary>
    List<ActorBase> _hitedActors;
    /// <summary>
    /// 所持碰撞盒子
    /// </summary>
    SkillCollider[] colliders;
    /// <summary>
    /// 技能表现控制器
    /// </summary>
    SkillTweenBase[] tweens;

    public override enSkillType type
    {
        get { return enSkillType.NormalSkill; }
    }

    void Awake()
    {
        _hitedActors = new List<ActorBase>();
        ///Collider 自带/或动态加入
        colliders = GetComponentsInChildren<SkillCollider>();
        tweens = GetComponentsInChildren<SkillTweenBase>();
    }

    public override void Play(enSkillConstructionData data)
    {
        base.Play(data);
        _CheackCollider();
        _hitedActors.Clear();
        for (int i = 0; i < tweens.Length; ++i)
        {
            tweens[i].Play();
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

    public override void OnCollided(ActorBase other, Transform colliderTrm)
    {
        base.OnCollided(other, colliderTrm);

        if (!repeatAttackSameTarget && _hitedActors.Contains(other))
            return;

        var collidedResult = (collidedWithSameCamp && _data.attacker.camp == other.camp) 
            || (!collidedWithSameCamp && _data.attacker.camp != other.camp);

        if (collidedResult)
        {
            if (!_hitedActors.Contains(other))
                _hitedActors.Add(other);

            _data.belongStateData.AddHiedActor(other);
            if(isHitOnTrigger)
            {
                for (int j = 0; j < buffId.Length; ++j)
                {
                    //other.addBuff(buffId[j]); 添加
                    var buffCenter = other.GetActorComponent<AC_BuffCenter>();
                    if(buffCenter != null)
                    {
                        buffCenter.AddBuff(buffId[j], data.attacker);
                    }
                }
            }
            for (int i = 0; i < tweens.Length; ++i)
            {
                tweens[i].OnCollided(colliderTrm);
            }
        }
    }

    public override void OnExitCollided(ActorBase other)
    {
        base.OnExitCollided(other);
        if (!_hitedActors.Contains(other))
            return;

        _hitedActors.Remove(other);
    }

    /// <summary>
    /// 为碰撞池内目标添加buff
    /// </summary>
    public override void AddBuff()
    {
        base.AddBuff();
        for (int i = 0; i < _hitedActors.Count; ++i)
        {
            if (maxHitByOnce > 0 && i > maxHitByOnce)
            {
                break;
            }
            for (int j = 0; j < buffId.Length; ++j)
            {
                //_hitedActors[i].addBuff(buffId[j]); 添加
                var buffCenter = _hitedActors[i].GetActorComponent<AC_BuffCenter>();
                if (buffCenter != null)
                {
                    buffCenter.AddBuff(buffId[j], data.attacker);
                }
            }
        }
    }

    public override void Finish()
    {
        for (int i = 0; i < tweens.Length; ++i )
        {
            tweens[i].OnFinish();
        }
        _SendAction();
        base.Finish();
    }
    void _SendAction()
    {
        switch(actionBackType)
        {
            case enSkillActionBackType.onHited:
                {
                    if (_hitedActors.Count == 0)
                        _data.belongStateData.OperationByType(enSkillReleaseState.enStateOperation.finish);
                    else
                        _data.belongStateData.OperationByType(enSkillReleaseState.enStateOperation.nextState);
                }
                break;
        }
    }
    void _CheackCollider()
    {
        if (colliderDelay <= 0)
        {
            for (int i = 0; i < colliders.Length; ++i)
            {
                colliders[i].enabled = true;
            }
            if(colliderLife > 0)
            {
                AddTimerEvent(delegate()
                {
                    for (int i = 0; i < colliders.Length; ++i)
                    {
                        colliders[i].enabled = false;
                    }
                }, colliderLife);
            }
        }
        else
        {
            for (int i = 0; i < colliders.Length; ++i)
            {
                colliders[i].enabled = false;
            }

            AddTimerEvent(delegate()
            {
                for (int i = 0; i < colliders.Length; ++i)
                {
                    colliders[i].enabled = true;
                }
                if (colliderLife > 0)
                {
                    AddTimerEvent(delegate()
                    {
                        for (int i = 0; i < colliders.Length; ++i)
                        {
                            colliders[i].enabled = false;
                        }
                    }, colliderLife);
                }
            }, colliderDelay);
        }
    }

}
