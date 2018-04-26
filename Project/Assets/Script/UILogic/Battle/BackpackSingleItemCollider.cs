using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 背包列表内单个元素的点触控制器
/// </summary>
public class BackpackSingleItemCollider : MonoBehaviour
{
    MyDragAndDropCtr _dragAndDrop;
    public MyDragAndDropCtr dragAndDrop
    {
        get
        {
            if (_dragAndDrop == null)
            {
                _dragAndDrop = gameObject.AddComponent<MyDragAndDropCtr>();
                _dragAndDrop.Begin(_OnPress, _OnDragDropStart, _OnDrag, _OnDragDropRelease, UIDragDropItem.Restriction.Horizontal);
            }
            return _dragAndDrop;
        }
    }
    BackpackSingleItemCtr _rootCtr;
    public BackpackSingleItemCtr rootCtr
    {
        get
        {
            if (_rootCtr == null)
                _rootCtr = GetComponentInParent<BackpackSingleItemCtr>();
            return _rootCtr;
        }
    }

    const float LONGTOUCHTIME = 0.5f;
    bool _touchState;
    bool _showState;
    float _touchTimer;
    BattleItemInfoUI _currentInfoUI;
    enItemData _data;
    ItemBoxUI _tempDragShow;
    Tweener _backTweener;

    public void ResetData(enItemData data)
    {
        _data = data;
        dragAndDrop.SetState(_data != null);
    }
    void Update()
    {
        _ShowInfoCheck(Time.deltaTime);
        if (_tempDragShow != null)
            _tempDragShow.transform.position = transform.position;
    }
    #region ShowInfo
    void _ShowStateChange(bool state)
    {
        _touchState = state;
        _touchTimer = LONGTOUCHTIME;
        if (!state)
            _SetDisplayInfo(false);
    }
    void _ShowInfoCheck(float dt)
    {
        if (_showState || !_touchState)
            return;
        _touchTimer -= dt;
        if (_touchTimer <= 0)
            _SetDisplayInfo(true);
    }
    void _SetDisplayInfo(bool state)
    {
        if (_showState == state)
            return;
        _showState = state;
        if (_showState && _data != null)
        {
            _currentInfoUI = UIMgr.Instance.GetUI<BattleItemInfoUI>();
            if(_data.firstType == enItemType.equipment)
            {
                var equipCtr = UIMgr.Instance.GetUI<EquipmentMainUI>();
                _currentInfoUI.ResetData(_data, equipCtr.GetDataByType((enEquipmentType)_data.lastType));
            }
            else
                _currentInfoUI.ResetData(_data, null);
        }
        else
            UIMgr.Instance.RemoveUI(_currentInfoUI);
    }
    #endregion
    #region Event
    void _OnPress(bool isPressed)
    {
        _ShowStateChange(isPressed);
    }
    void _OnDragDropStart()
    {
        _ShowStateChange(false);
        var obj = ObjManager.Instance.addChild("", GamePrefab.ItemBoxUI, UIMgr.Instance.GetUI<TempTopPanel>().transform, true);
        UIMgr.Instance.GetUI<TempTopPanel>().childCount++;
        _tempDragShow = obj.GetComponent<ItemBoxUI>();
        _tempDragShow.Reset(_data);
        if (_data.firstType == enItemType.equipment)
            UIMgr.Instance.GetUI<EquipmentMainUI>().SetDisplay(true);
    }
    void _OnDrag(Vector2 delta)
    {

    }
    void _OnDragDropRelease(GameObject surface)
    {
        //TODO:判断该操作是否有效
        if(surface != null)
        {
            var ctr = surface.GetComponent<EquipmentSingleCollider>();
            if (ctr != null && _data.firstType == enItemType.equipment)
            {
                var result = ctr.rootCtr.ResetData(_data);
                if (result)
                {
                    //TODO:修改装备装备 移除背包内该装备显示
                    Debug.Log("");
                }
                else
                    UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.operation, "请装备到" + Contrastting.GetEquipmentName((enEquipmentType)_data.lastType) + "槽");
                _DropResult(result);
                return;
            }
        }
        _DropResult(false);
    }
    void _OnBackComplete()
    {
        ObjManager.Instance.Despawn(_tempDragShow.gameObject);
        UIMgr.Instance.GetUI<TempTopPanel>().childCount--;
        _tempDragShow = null;
    }
    void _DropResult(bool state)
    {
        if (state)
        {
            transform.localPosition = Vector3.zero;
            _OnBackComplete();
        }
        else
        {
            _backTweener = transform.DOLocalMove(Vector3.zero, 0.5f);
            _backTweener.OnComplete(_OnBackComplete);
        }
    }
    #endregion
	
}
