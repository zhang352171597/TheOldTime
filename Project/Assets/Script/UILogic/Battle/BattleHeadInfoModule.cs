using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BattleHeadInfoModule : MonoBehaviour 
{
    Dictionary<string, ActorHeadInfoCtr> _dic;
    Dictionary<string, ActorHeadInfoCtr> dic
    {
        get 
        {
            if (_dic == null)
                _dic = new Dictionary<string, ActorHeadInfoCtr>();
            return _dic;
        }
    }


    public ActorHeadInfoCtr getHeadInfo(ActorBase actor)
    {
        if (dic.ContainsKey(actor.UID))
            return dic[actor.UID];

        var obj = ObjManager.Instance.addChild(GamePath.UIPrefabs + "Tools/", "ActorHeadInfo", transform, true);
        var tempCtr = obj.GetComponent<ActorHeadInfoCtr>();
        tempCtr.Begin(actor.camp , actor.headInfoPos);
        dic.Add(actor.UID, tempCtr);
        return tempCtr;
    }
    
    public void Despawn(string actorUID)
    {
        
        if (dic.ContainsKey(actorUID))
        {
            var obj = dic[actorUID];
            ObjManager.Instance.Despawn(obj.gameObject);
            dic.Remove(actorUID);
        }
        else
            EditorDebug.LogWarning("尝试移出一个未缓存的角色头标, ActorUID: " + actorUID);
    }

}
