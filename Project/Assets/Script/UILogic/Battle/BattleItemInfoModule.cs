using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleItemInfoModule : MonoBehaviour 
{
    public UISprite icon;
    public UILabel name;
    public UILabel quality;
    public UIGrid grid;
    public Transform infoOffset;
    public UILabel itemInfo;
    public UILabel skillInfo;
    List<UILabel> _labPool;
    List<UILabel> labPool
    {
        get
        {
            if (_labPool == null)
                _labPool = new List<UILabel>();
            return _labPool;
        }
    }
    public void ResetData(enItemData data)
    {
        if (data != null)
        {
            icon.spriteName = data.icon;
            name.text = data.nameWithColor;
            quality.text = data.qualityWithColor;
            var attributeCount = data.attributeData == null ? 0 : data.attributeData.attributes.Length;
            infoOffset.localPosition = -Vector3.up * (5 + 20 * attributeCount);
            itemInfo.text = data.info;
            _RefreshEquipAtt(attributeCount , data);
            gameObject.SetActive(true);
        }
        else
            gameObject.SetActive(false);
    }
    void _RefreshEquipAtt(int attCount, enItemData data)
    {
        Debug.Log("属性数量   " + attCount);
        if (attCount > 0)
        {
            var tempCount = 0;
            for (int i = 0; i < attCount; ++i)
            {
                UILabel lab = null;
                if (tempCount < labPool.Count)
                {
                    lab = labPool[tempCount];
                    lab.gameObject.SetActive(true);
                }
                else
                {
                    var obj = ObjManager.Instance.addChild(GamePath.UIPrefabs + "Tools/" , "LabPrefab", grid.transform, true);
                    lab = obj.GetComponent<UILabel>();
                    labPool.Add(lab);
                }
                lab.gameObject.name = "att_" + i;
                var d = data.attributeData.attributes[i];
                lab.text = Contrastting.qualityOfText[d.quality] + Contrastting.equipAttrbuteName[d.key] + "  " + d.value;
                tempCount++;
            }
            ///无效字段隐藏
            for (int i = 0; i < labPool.Count; ++i)
            {
                if (tempCount <= i)
                    labPool[i].gameObject.SetActive(false);
            }
            grid.Reposition();
            grid.gameObject.SetActive(true);
        }
        else
            grid.gameObject.SetActive(false);
        
        //关于特技
        if (data.attributeData != null && data.attributeData.carrySkill.isValid)
        {
            skillInfo.text = "特技  " + data.attributeData.carrySkill.skillInfo;
            skillInfo.gameObject.SetActive(true);
        }
        else
            skillInfo.gameObject.SetActive(false);
    }

}
