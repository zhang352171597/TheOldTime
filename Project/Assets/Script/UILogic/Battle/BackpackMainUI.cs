using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 背包UI模块
/// </summary>
public class BackpackMainUI : UIBase
{
    public UIScrollView scrollView;
    public UIGrid grid;
    public GameObject itemPrefab;
    public UILabel stateTip;
    public Animator stateAni;
    public UILabel soulLab;

    /// <summary>
    /// 当前显示状态
    /// </summary>
    bool _displayState;
    /// <summary>
    /// 背包列表
    /// </summary>
    List<BackpackSingleItemCtr> _boxs;
    List<BackpackSingleItemCtr> boxs
    {
        get
        {
            if (_boxs == null)
                _boxs = new List<BackpackSingleItemCtr>();
            return _boxs;
        }
    }

    void Awake()
    {
        UIEventListener.Get(stateTip.parent.gameObject).onClick = _OnClickState;
    }

    public void Load()
    {
        if (boxs.Count == 0)
        {
            grid.repositionNow = true;
            _InitItemBox(BackpackData.GRIDCOUNT);
            UIMgr.Instance.RemoveUI(this);
        }
    }

    void _InitItemBox(int maxCount)
    {
        for(int i = 0 ; i < maxCount; ++i)
        {
            var obj = ObjManager.Instance.addChild(itemPrefab, grid.transform, true);
            obj.name = "itemBox_" + i;
            var ctr = obj.GetComponent<BackpackSingleItemCtr>();
            ctr.Begin(scrollView);
            boxs.Add(ctr);
        }
        grid.Reposition();
    }
    void _OnShow()
    {
        DataCenter.Instance.backpackData.onChangeItemCallback += _OnItemChanged;
        DataCenter.Instance.backpackData.onChangeSoulCallback += _OnSoulChanged;
        if (DataCenter.Instance.backpackData.changedCache.Count == 0)
            return;
        var dic = DataCenter.Instance.backpackData.changedCache;
        foreach(var d in dic.Values)
        {
            if(d.backpackIndex < boxs.Count)
            {
                boxs[d.backpackIndex].ResetData(d);
            }
        }
        _OnSoulChanged(DataCenter.Instance.backpackData.soul);
        DataCenter.Instance.backpackData.changedCache.Clear();
    }
    void _OnHide()
    {
		UIMgr.Instance.GetUI<EquipmentMainUI> ().SetDisplay (false);
        DataCenter.Instance.backpackData.onChangeItemCallback -= _OnItemChanged;
        DataCenter.Instance.backpackData.onChangeSoulCallback -= _OnSoulChanged;
    }
    void _OnItemChanged(enItemData d)
    {
        if (d.backpackIndex < boxs.Count)
        {
            boxs[d.backpackIndex].ResetData(d);
        }
    }
    void _OnSoulChanged(int count)
    {
        soulLab.text = count.ToString();
    }
    void _OnClickState(GameObject btn)
    {
        SetDisplay(!_displayState);
    }
    public void SetDisplay(bool state)
    {
        if (state == _displayState)
            return;
        _displayState = state;
        stateAni.SetTrigger(_displayState ? "show" : "hide");
        stateTip.text = _displayState ? ">>" : "<<";
        if (_displayState)
            _OnShow();
        else
            _OnHide();
    }
}
