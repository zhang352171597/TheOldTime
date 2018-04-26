using UnityEngine;
using System.Collections;

/// <summary>
/// 装备信息栏单个元素控制器
/// </summary>
public class EquipmentSingleItemCtr : MonoBehaviour 
{
    public UISprite icon;
    public EquipmentSingleCollider colliderCtr;
    public enEquipmentType equipType;
    enItemData _data;
    public enItemData data
    { get { return _data; } }

    public bool ResetData(enItemData data)
    {
        if(data == null)
        {
            _data = null;
            colliderCtr.ResetData(data);
            icon.gameObject.SetActive(false);
            DataCenter.Instance.userData.ChangeEquip(equipType, data);
            return true;
        }
        else
        {
            if (data.lastType == (int)equipType)
            {
                _data = data;
                colliderCtr.ResetData(data);
                icon.gameObject.SetActive(true);
                icon.spriteName = data.icon;
                DataCenter.Instance.userData.ChangeEquip(equipType, data);
                return true;
            }
            else
                return false;
        }
    }
    public void ChangeItemDisplay(bool state)
    {
        if (state && _data != null)
            icon.gameObject.SetActive(true);
        else if(!state)
            icon.gameObject.SetActive(false);
    }
}
