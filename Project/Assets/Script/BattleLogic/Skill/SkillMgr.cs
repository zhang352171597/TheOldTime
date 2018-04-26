using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillMgr : ModuleComponent<SkillMgr> 
{

    List<SingleSkillBase> _skillPool;
    List<SingleSkillBase> _skillRemoveCachePool;

    public void Load()
    {
        _skillPool = new List<SingleSkillBase>();
        _skillRemoveCachePool = new List<SingleSkillBase>();
    }

    public void Begin()
    {

    }

	public void CreateSkill(ActorBase attacker , enSkillReleaseState releaseState)
    {
        var data = new enSkillConstructionData();
        data.belongStateData = releaseState;
        data.attacker = attacker;
        var obj = ObjManager.Instance.addChild(GamePath.SkillPrefabs, releaseState.singleData.prefabs, transform);
        var ctr = obj.GetComponent<SingleSkillBase>();
        ctr.Play(data);
        _skillPool.Add(ctr);
    }

    public void DespawnSkill(SingleSkillBase skill)
    {
        if (!_skillRemoveCachePool.Contains(skill))
            _skillRemoveCachePool.Add(skill);
    }

    public void UpdateLogic(float dt)
    {
        for(int i = 0 ; i < _skillPool.Count; ++i)
        {
            _skillPool[i].UpdateLogic(dt);
        }

        for(int i = 0 ; i < _skillRemoveCachePool.Count; ++i)
        {
            var tempCtr = _skillRemoveCachePool[i];
            if (_skillPool.Contains(tempCtr))
            {
                _skillPool.Remove(tempCtr);
                ObjManager.Instance.Despawn(tempCtr.gameObject);
            }
        }

        if (_skillRemoveCachePool.Count > 0)
            _skillRemoveCachePool.Clear();
    }

    //test
    void Update()
    {
        UpdateLogic(Time.deltaTime);
    }



}
