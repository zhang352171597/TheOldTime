using UnityEngine;
using System.Collections;

/// <summary>
/// 获取一个最新的panel承载UI节点，所该panel下节点全无则回收该panel
/// </summary>
public class TempTopPanel : UIBase 
{
    int _childCount;
    public int childCount
    {
        get { return _childCount; }
        set 
        { 
            _childCount = value;
            if (value <= 0)
            {
                _childCount = 0;
                UIMgr.Instance.RemoveUI(this);
            }
        }
    }

}
