using UnityEngine;
using System.Collections;
using System;

public class MyDragAndDropCtr : UIDragDropItem
{
    Action<bool> _onPressedFunc;
    Action _onDragStartFunc;
    Action<Vector2> _onDragFunc;
    Action<GameObject> _onDropReleaseFunc;

    public void Begin(Action<bool> onPressedFunc, Action onDragStartFunc 
        , Action<Vector2> onDragFunc , Action<GameObject> onDropReleaseFunc , Restriction dragType)
    {
        _onPressedFunc = onPressedFunc;
        _onDragStartFunc = onDragStartFunc;
        _onDragFunc = onDragFunc;
        _onDropReleaseFunc = onDropReleaseFunc;
        restriction = dragType;
    }
    public void SetState(bool state)
    {
        enabled = state;
    }
    protected override void OnPress(bool isPressed)
    {
        base.OnPress(isPressed);
        if (_onPressedFunc != null)
            _onPressedFunc(isPressed);
    }
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        if (_onDragStartFunc != null)
            _onDragStartFunc();
    }
    protected override void OnDrag(Vector2 delta)
    {
        base.OnDrag(delta);
        if (_onDragFunc != null)
            _onDragFunc(delta);
    }
    protected override void OnDragDropRelease(GameObject surface)
    {
        base.OnDragDropRelease(surface);
        ///PS：  OnDragDropRelease在OnPress之前，若setActive false ，OnPress会被吞掉。 
        ///Drag开关无法重置导致下一次拖动无效！ 因此手动重置
        mPressed = false;
        mTouch = null;
        if (_onDropReleaseFunc != null)
            _onDropReleaseFunc(surface);
    }
}
