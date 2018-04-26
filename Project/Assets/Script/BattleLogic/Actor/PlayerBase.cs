using UnityEngine;
using System.Collections;

public class PlayerBase : ActorBase 
{
    public override void Init(enActorConstructionData data)
    {
        base.Init(data);
        AddActorComponent<AC_OperationCtr>();
        AddActorComponent<AC_StateCtrBase>();
        AddActorComponent<AC_PlayerSkillHand>();
        AddActorComponent<AC_CameraFollow>();
        AddActorComponent<AC_HeadInfoCtr>();
        AddActorComponent<AC_BuffCenter>();
        UIMgr.Instance.GetUI<BackpackMainUI>();
        UIMgr.Instance.GetUI<EquipmentMainUI>();
        MessageCenter.Instance.Bind(enGameMessageType.setPlayerPosForce, _SetPosition);
    }
    protected override void _InitAttribute()
    {
        base._InitAttribute();
        DataCenter.Instance.userData.onUserAttributeChange += _OnChangeAttrubute;
        var attrebuteDic =  DataCenter.Instance.userData.attributeDic;
        foreach(var att in attrebuteDic)
        {
            if(att.Key == enUserAttribute.hp)
            {
                coreAttribute.AddAttrbute(ActorAttributeTag.maxHp, att.Value.Value, _OnHpMaxChange);
				coreAttribute.AddAttrbute(ActorAttributeTag.hp, att.Value.Value, _OnHpChange , ActorAttributeTag.maxHp);
            }
            else
                coreAttribute.AddAttrbute(att.Key.ToString(), att.Value.Value, null);
        }
    }
    void _OnChangeAttrubute()
    {
        var attrebuteDic = DataCenter.Instance.userData.attributeDic;
        foreach (var att in attrebuteDic)
        {
            if (att.Key == enUserAttribute.hp)
                coreAttribute.ResetBaseValue(ActorAttributeTag.maxHp, att.Value.Value);
            else
                coreAttribute.ResetBaseValue(att.Key.ToString(), att.Value.Value);
        }
    }
    public override void Despawn()
    {
        DataCenter.Instance.userData.onUserAttributeChange -= _OnChangeAttrubute;
        MessageCenter.Instance.UnBind(enGameMessageType.setPlayerPosForce, _SetPosition);
        base.Despawn();
    }
    #region Callback
    void _SetPosition(params System.Object[] objs)
    {
        position = (Vector3)objs[0];
    }
    #endregion
}
