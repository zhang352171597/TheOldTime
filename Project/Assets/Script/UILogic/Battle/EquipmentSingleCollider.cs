using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// 装备信息内单个元素的点触控制器
/// </summary>
public class EquipmentSingleCollider : MonoBehaviour 
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
    EquipmentSingleItemCtr _rootCtr;
    public EquipmentSingleItemCtr rootCtr
    {
        get
        {
            if (_rootCtr == null)
                _rootCtr = GetComponentInParent<EquipmentSingleItemCtr>();
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
            SetDisplayInfo(false);
    }
    void _ShowInfoCheck(float dt)
    {
        if (_showState || !_touchState)
            return;
        _touchTimer -= dt;
        if (_touchTimer <= 0)
            SetDisplayInfo(true);
    }
    void SetDisplayInfo(bool state)
    {
        if (_showState == state)
            return;
        _showState = state;
        if (_showState && _data != null)
        {
            _currentInfoUI = UIMgr.Instance.GetUI<BattleItemInfoUI>();
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
        rootCtr.ChangeItemDisplay(false);
        UIMgr.Instance.GetUI<BackpackMainUI>().SetDisplay(true);
    }
    void _OnDrag(Vector2 delta)
    {

    }
    void _OnDragDropRelease(GameObject surface)
    {
        //TODO:判断该操作是否有效
        if (surface != null)
        {
            var ctr = surface.GetComponentInParent<BackpackMainUI>();
            if (ctr != null)
            {
                _DropResult(true);
                rootCtr.ResetData(null);
                //TODO:修改装备状态
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
        rootCtr.ChangeItemDisplay(true);
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
