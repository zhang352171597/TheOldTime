using UnityEngine;
using System.Collections;

public class MapActorSetting : MonoBehaviour 
{
    public string ID = "14101";
    public enCamp camp = enCamp.enemy;

    ActorBase _actor;
    public void Create()
    {
        enActorConstructionData data = new enActorConstructionData();
        data.camp = camp;
        data.type = (enActorType)JSONModel.getInstance().GetType(ID);
        data.id = ID;
        _actor = ActorMgr.Instance.CreateActor(data);
        _actor.position = transform.position;
        _actor.forward = transform.forward;
    }
    public void Despawn()
    {
        _actor.Despawn();
    }
}
