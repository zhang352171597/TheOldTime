using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleSkillModuleCtr : MonoBehaviour {

    Dictionary<int, BattleSkillButtonCtr> _buttonDic;
    /// <summary>
    /// 设置技能数据与UI的对应关系
    /// </summary>
    /// <param name="relaseDatas"></param>
    public void Reset(List<enSkillReleaseData> relaseDatas)
    {
        if (_buttonDic == null)
        {
            _buttonDic = new Dictionary<int, BattleSkillButtonCtr>();
            var buttons = GetComponentsInChildren<BattleSkillButtonCtr>();
            for(int i = 0 ; i < buttons.Length; ++i)
            {
                _buttonDic.Add(i, buttons[i]);
            }
        }
        for (int i = 0; i < relaseDatas.Count; ++i )
        {
            ChangeSkill(i, relaseDatas[i]);
        }
    }
    public void ChangeSkill(int slotIndex , enSkillReleaseData data)
    {
        if (_buttonDic.ContainsKey(slotIndex))
            {
                _buttonDic[slotIndex].Init(data);
            }
            else
                EditorDebug.LogError("技能槽错误");
    }
}
