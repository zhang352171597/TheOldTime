using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface ISingleSkill
{
    void Play(enSkillConstructionData data);
    void Finish();
    void UpdateLogic(float dt);
    void OnCollided(ActorBase other, Transform colliderTrm);
    void OnExitCollided(ActorBase other);
}

public class SingleSkillBase : MonoTimer, ISingleSkill
{
    public virtual enSkillType type
    {
        get { return enSkillType.Base; }
    }

    /// <summary>
    /// 自身构造数据
    /// </summary>
    protected enSkillConstructionData _data;
    public enSkillConstructionData data
    {
        get { return _data; }
    }

    Vector3 _position;
    public Vector3 position
    {
        get { return _position; }
    }

    public virtual void Play(enSkillConstructionData data)
    {
        TimerBegin();
        _data = data;
        _position = data.attacker.position + data.attacker.bodyOffset + data.attacker.forward * data.belongStateData.singleData.bornPosOffset;
    }

    public virtual void Finish()
    {
        SkillMgr.Instance.DespawnSkill(this);
    }

    public virtual void UpdateLogic(float dt)
    {
        TimerUpdate(dt);
    }

    public virtual void OnCollided(ActorBase other, Transform colliderTrm)
    {
    }

    public virtual void OnExitCollided(ActorBase other)
    {

    }

    public virtual void AddBuff()
    {

    }

}


