using UnityEngine;
using System.Collections;

/// <summary>
/// 角色掉落控制器
/// </summary>
public class AC_Dropping : ActorComponentBase
{
    enDropData _dropData;

    public override void OnAwake()
    {
        base.OnAwake();
        actor.Bind(enActorMessageType.tryDropItem, _OnDropping);
        var dropStr = JSONModel.getInstance().GetDrop(actor.id);
        _dropData = new enDropData(dropStr);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        actor.UnBind(enActorMessageType.tryDropItem, _OnDropping);
    }
    void _OnDropping(params System.Object[] objs)
    {
        var monsterDrops = _dropData.dropResult;
        for (int i = 0; i < monsterDrops.Count; ++i)
        {
            DropOutMgr.Instance.CreateItem(monsterDrops[i], actor.position, null);
        }
        var mapDrops = MapMgr.Instance.dropResult;
        for (int i = 0; i < mapDrops.Count; ++i)
        {
            DropOutMgr.Instance.CreateItem(mapDrops[i], actor.position, null);
        }
    }
}
