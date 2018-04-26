using UnityEngine;
using System.Collections;

public class MonsterBase : ActorBase 
{
    public override void Init(enActorConstructionData data)
    {
        base.Init(data);
        AddActorComponent<AC_StateCtrBase>();
        AddActorComponent<AC_HeadInfoCtr>();
        AddActorComponent<AC_BuffCenter>();
        AddActorComponent<AC_Dropping>();
        AddActorComponent<AC_ShaderCtr>();
    }
    protected override void _InitAttribute()
    {
        base._InitAttribute();
        coreAttribute.AddAttrbute(ActorAttributeTag.maxHp, JSONModel.getInstance().GetHP(id), _OnHpMaxChange);
        coreAttribute.AddAttrbute(ActorAttributeTag.hp, coreAttribute.GetAttribute(ActorAttributeTag.maxHp).baseValue, _OnHpChange, ActorAttributeTag.maxHp);
        coreAttribute.AddAttrbute(ActorAttributeTag.ad, JSONModel.getInstance().GetATT(id), null);
        coreAttribute.AddAttrbute(ActorAttributeTag.df, JSONModel.getInstance().GetDEF(id), null);
        coreAttribute.AddAttrbute(ActorAttributeTag.shield, 0, _OnHpChange);
        coreAttribute.AddAttrbute(ActorAttributeTag.speed, 5, null);
    }
}
