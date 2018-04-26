using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 技能操作链接
/// </summary>
public class AC_PlayerSkillHand : ActorComponentBase
{
    List<enSkillReleaseData> _releaseDataPool;
    public override void OnAwake()
    {
        //初始技能数据
        _releaseDataPool = new List<enSkillReleaseData>();
        var skillDic = DataCenter.Instance.userData.skillDic;
        for (int i = 0; i < 4; ++i )
        {
            var data = new enSkillReleaseData(skillDic[i]);
            _releaseDataPool.Add(data);
        }
        _ReBindData();
        DataCenter.Instance.userData.onUserSkillChange += _OnSkillChange;
    }
    public override void OnDestroy()
    {
        DataCenter.Instance.userData.onUserSkillChange -= _OnSkillChange;
    }
    public override void LogicUpdate(float dt)
    {
        for(int i = 0 ; i < _releaseDataPool.Count; ++i)
        {
            _releaseDataPool[i].UpdateLogic(dt);
        }
    }
    void _OnSkillRealese(enSkillReleaseState releaseState)
    {
        DeBugger.Log("释放一个技能表现：  " + releaseState.singleData.prefabs);
    }
    void _OnSkillChange()
    {
        var isChanged = false;
        var skillDic = DataCenter.Instance.userData.skillDic;
        foreach(var s in skillDic)
        {
            if (_releaseDataPool[s.Key].id != s.Value)
            {
                var lastID = _releaseDataPool[s.Key].id;
                var content = "卸载技能: " + Contrastting.qualityOfText[4] + JSONSkillDic.dicById[lastID][JSONTag.name];
                UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.getProps, content);
                _releaseDataPool[s.Key] = new enSkillReleaseData(s.Value);
                content = "装载技能: " + Contrastting.qualityOfText[4] + JSONSkillDic.dicById[s.Value][JSONTag.name];
                UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.getProps, content);
                isChanged = true;
            }
        }
        if (isChanged)
            _ReBindData();
    }
    void _ReBindData()
    {
        //绑定释放回调
        for (int i = 0; i < _releaseDataPool.Count; ++i)
        {
            _releaseDataPool[i].SetReleaseBind(_OnSkillRealese);
        }
        UIMgr.Instance.GetUI<BattleMainUI>().skillModule.Reset(_releaseDataPool);
    }
}
