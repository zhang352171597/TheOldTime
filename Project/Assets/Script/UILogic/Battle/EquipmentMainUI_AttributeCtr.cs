using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentMainUI_AttributeCtr : MonoBehaviour
{
    public UIGrid grid;
    List<UILabel> _labPool;
    public List<UILabel> labPool
    {
        get
        {
            if (_labPool == null)
                _labPool = new List<UILabel>();
            return _labPool;
        }
    }
    public void OnDisplayChange(bool state)
    {
        if (state)
        {
            _RefreshAttribute();
            DataCenter.Instance.userData.onUserAttributeChange += _RefreshAttribute;
        }
        else
            DataCenter.Instance.userData.onUserAttributeChange -= _RefreshAttribute;
    }
    void _RefreshAttribute()
    {
        var atts = DataCenter.Instance.userData.attributeDic;
        var index = -1;
        foreach(var a in atts)
        {
            index++;
            UILabel lab = null;
            if (index < labPool.Count)
            {
                lab = labPool[index];
                lab.gameObject.SetActive(true);
            }
            else
            {
                var obj = ObjManager.Instance.addChild(GamePath.UIPrefabs + "Tools/", "LabPrefab", grid.transform, true);
                lab = obj.GetComponent<UILabel>();
                labPool.Add(lab);
            }
            lab.text = Contrastting.equipAttrbuteName[(int)a.Key] + " " + a.Value.Value;
        }

        for(int i = 0 ; i < labPool.Count; ++i)
        {
            if(i > index)
            {
                labPool[i].gameObject.SetActive(false);
            }
        }
        grid.Reposition();
    }
}
