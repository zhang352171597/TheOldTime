using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 装备信息界面
/// </summary>
public class EquipmentMainUI : UIBase 
{
    public UILabel stateTip;
    public Animator stateAni;
    public UILabel name;
    public UILabel lv;
    public UILabel lvPhase;
    public UISprite expBar;
    public EquipmentMainUI_AttributeCtr attributeCtr;

    Dictionary<enEquipmentType, EquipmentSingleItemCtr> _slotDic;
    Dictionary<enEquipmentType, EquipmentSingleItemCtr> slotDic
    {
        get
        {
            if (_slotDic == null)
            {
                _slotDic = new Dictionary<enEquipmentType, EquipmentSingleItemCtr>();
                var objs = GetComponentsInChildren<EquipmentSingleItemCtr>();
                for(int i = 0 ; i < objs.Length; ++i)
                {
                    _slotDic.Add(objs[i].equipType, objs[i]);
                }
            }
                return _slotDic;
        }
    }
    /// <summary>
    /// 当前显示状态
    /// </summary>
    bool _displayState;
    public void Load()
    {
    }
    void Awake()
    {
        UIEventListener.Get(stateTip.parent.gameObject).onClick = _OnClickState;
    }
    void _OnShow()
    {
        attributeCtr.OnDisplayChange(true);
    }
    void _OnHide()
    {
        attributeCtr.OnDisplayChange(false);
    }
    void _OnClickState(GameObject btn)
    {
        SetDisplay(!_displayState);
    }
    public void SetDisplay(bool state)
    {
        if (_displayState != state)
        {
            _displayState = state;
            stateAni.SetTrigger(_displayState ? "show" : "hide");
            stateTip.text = _displayState ? "<<" : ">>";
            if (_displayState)
                _OnShow();
            else
                _OnHide();
        }
    }
    /// <summary>
    /// 获取到对应插槽当前装备
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public enItemData GetDataByType(enEquipmentType type)
    {
        if (slotDic.ContainsKey(type))
            return slotDic[type].data;
        else
            return null;
    }
}
