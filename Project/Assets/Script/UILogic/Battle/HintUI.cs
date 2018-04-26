using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 游戏提示模块
/// </summary>
public class HintUI : UIBase
{
    public struct strHintData
    {
        public eHintType type;
        public string content;
        public float delayHinde;
    }
    public enum eHintType
    {
        /// <summary>
        /// 获取道具
        /// </summary>
        getProps,
        /// <summary>
        /// 地图名称
        /// </summary>
        mapName,
        /// <summary>
        /// 操作提示
        /// </summary>
        operation,
    }
    float _mapHintTimer;
    List<strHintData> _mapList = new List<strHintData>();
    float _propHintTimer;
    List<strHintData> _propList = new List<strHintData>();
    float _operationHintTimer;
    List<strHintData> _operationList = new List<strHintData>();

    public void ShowHint(eHintType type, string content , float delayHide = 1)
    {
        strHintData data;
        data.type = type;
        data.content = content;
        data.delayHinde = delayHide;
        switch (data.type)
        {
            case eHintType.getProps:
                _propList.Add(data);
                break;
            case eHintType.mapName:
                _mapList.Add(data);
                break;
            case eHintType.operation:
                _operationList.Add(data);
                break;
        }
    }
    void Update()
    {
        _CheackTime(Time.deltaTime);
    }
    void _CheackTime(float dt)
    {
        if (_propHintTimer > 0)
            _propHintTimer -= dt;
        if (_propList.Count > 0)
        {
            if (_propHintTimer <= 0)
            {
                _ShowHint(_propList[0]);
                _propList.RemoveAt(0);
                _propHintTimer = 0.5f;
            }
        }
        if (_mapHintTimer > 0)
            _mapHintTimer -= dt;
        if (_mapList.Count > 0)
        {
            if (_mapHintTimer <= 0)
            {
                _ShowHint(_mapList[0]);
                _mapList.RemoveAt(0);
                _mapHintTimer = 3;
            }
        }
        if (_operationHintTimer > 0)
            _operationHintTimer -= dt;
        if (_operationList.Count > 0)
        {
            if (_operationHintTimer <= 0)
            {
                _ShowHint(_operationList[0]);
                _operationList.RemoveAt(0);
                _operationHintTimer = 1;
            }
        }
    }
    void _ShowHint(strHintData data)
    {
        var obj = ObjManager.Instance.addChild("", GamePrefab.HintItem, transform, true);
        var delayCtr = obj.GetComponent<DespawnByTime>();
        var lab = obj.GetComponentInChildren<UILabel>();
        switch (data.type)
        {
            case eHintType.getProps:
                {
                    obj.transform.DOKill();
                    lab.fontSize = 20;
                    obj.transform.DOLocalMoveY(150, data.delayHinde);
                }
                break;
            case eHintType.mapName:
                {
                    lab.fontSize = 35;
                }
                break;
            case eHintType.operation:
                {
                    lab.fontSize = 35;
                }
                break;
        }
        lab.text = data.content;
        delayCtr.Begin(data.delayHinde);
    }
}
