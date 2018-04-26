using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorMgr : ModuleComponent<ActorMgr> 
{
    ActorBase _player;
    public ActorBase player
    {
        get {return _player;}
    }

    List<ActorBase> _actorsPool;
    List<ActorBase> _actorsAddCache;
    List<ActorBase> _actorsRemoveCache;

    int _currentActorUIDIndex;
    public int currentActorUID
    {
        get { return _currentActorUIDIndex++; }
    }

    public void Load()
    {
        _actorsPool = new List<ActorBase>();
        _actorsAddCache = new List<ActorBase>();
        _actorsRemoveCache = new List<ActorBase>();
        _currentActorUIDIndex = 0;
    }

    public void Begin()
    {
        CreatePlayer();
    }

    public void CreatePlayer()
    {
        enActorConstructionData data = new enActorConstructionData();
        data.camp = enCamp.player;
        data.type = enActorType.boss;
        data.id = "14000";
        ActorMgr.Instance.CreateActor(data);
    }

    public ActorBase CreateActor(enActorConstructionData data)
    {
        ActorBase tempCtr = null;
        var actorObj = new GameObject();
        actorObj.transform.parent = transform;
        var modelType = JSONModel.getInstance().GetType(data.id);
        switch((enActorType)modelType)
        {
            case enActorType.player:
                tempCtr = actorObj.AddComponent<PlayerBase>();
            break;
            case enActorType.monster:
            tempCtr = actorObj.AddComponent<MonsterBase>();
            break;
            case enActorType.boss:
            tempCtr = actorObj.AddComponent<MonsterBase>();
            break;
            case enActorType.build:
            tempCtr = actorObj.AddComponent<ActorBase>();
            break;
        }
        if(tempCtr != null)
        {
            tempCtr.Init(data);
            return _CheackActor(tempCtr);
        }
        return null;
    }

    ActorBase _CheackActor(ActorBase actor)
    {
        if (actor.camp == enCamp.player && actor.type == enActorType.boss)
            _player = actor;

        _actorsAddCache.Add(actor);
        return actor;
    }

    public void UpdateLogic(float dt)
    {
        for(int i = 0 ; i < _actorsPool.Count; ++i)
        {
            _actorsPool[i].UpdateLogic(dt);
        }

        if(_actorsAddCache.Count > 0)
        {
            for(int i = 0 ; i < _actorsAddCache.Count; ++i)
            {
                _actorsPool.Add(_actorsAddCache[i]);
            }
            _actorsAddCache.Clear();
        }

        if(_actorsRemoveCache.Count > 0)
        {
            for (int i = 0; i < _actorsRemoveCache.Count; ++i)
            {
                _actorsPool.Remove(_actorsRemoveCache[i]);
                GameObject.Destroy(_actorsRemoveCache[i].gameObject);
            }
            _actorsRemoveCache.Clear();
        }
    }

    public void DespawnActor(ActorBase actor)
    {
        _actorsRemoveCache.Add(actor);
    }


}
