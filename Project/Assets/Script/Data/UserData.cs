using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class UserData
{
    MyInt _lv;
    public MyInt lv
    {
        get
        {
            if (_lv == null)
                _lv = new MyInt();
            return _lv;
        }
    }
    MyInt _exp;
    public MyInt exp
    {
        get
        {
            if (_exp == null)
                _exp = new MyInt();
            return _exp;
        }
    }
    /// <summary>
    /// 角色装备数据 位置，物品数据
    /// </summary>
    Dictionary<enEquipmentType, enItemData> _equipments;
    public Dictionary<enEquipmentType, enItemData> equipments
    {
        get
        {
            if (_equipments == null)
                _equipments = new Dictionary<enEquipmentType, enItemData>();
            return _equipments;
        }
    }
    /// <summary>
    /// 角色属性数据 类型，值
    /// </summary>
    Dictionary<enUserAttribute, MyInt> _attributeDic;
    public Dictionary<enUserAttribute, MyInt> attributeDic
    {
        get
        {
            if (_attributeDic == null)
            {
                _attributeDic = new Dictionary<enUserAttribute, MyInt>();
                for(int i = 1; i < (int)enUserAttribute.Count; ++i)
                {
                    _attributeDic.Add((enUserAttribute)i, new MyInt());
                }
            }
            return _attributeDic;
        }
    }

    /// <summary>
    /// 角色各等级默认的技能
    /// </summary>
    Dictionary<int, string[]> _defaultSkillDic;
    /// <summary>
    /// 角色技能数据 位置，技能ID
    /// </summary>
    Dictionary<int, string> _skillDic;
    public Dictionary<int, string> skillDic
    {
        get
        {
            if (_skillDic == null)
                _skillDic = new Dictionary<int, string>();
            return _skillDic;
        }
    }
    System.Action _onUserAttributeChange;
    /// <summary>
    /// 当角色属性发生变化
    /// </summary>
    public System.Action onUserAttributeChange
    {
        get { return _onUserAttributeChange; }
        set { _onUserAttributeChange = value; }
    }
    System.Action _onUserSkillChange;
    public System.Action onUserSkillChange
    {
        get { return _onUserSkillChange; }
        set { _onUserSkillChange = value; }
    }

    public void Load()
    {
        /*backpackDic.Clear();
        var tempStr = PlayerPrefs.GetString(UserTag.userData, "");
        if (tempStr.Length > 0)
        {
            var rootNode = JSONNode.Parse(tempStr);
            _newItemUID = rootNode[BackpackTag.newItemUID].AsInt;
            var packNode = rootNode[BackpackTag.packList];
            for (int i = 0; i < packNode.Count; ++i)
            {
                var d = new enItemData(packNode[i]);
                backpackDic.Add(d.backpackIndex, d);
            }
        }*/
        lv.baseValue = 1;
        ReLoadBaseAttribute();
        _ReLoadAdditionAttributeByEquip();
        _ReLoadSkill();
    }
    public void Save()
    {
        /*ListJsonData data = ListJsonData.Init();
        data.Add(BackpackTag.newItemUID, _newItemUID);
        ArrJsonData packList = ArrJsonData.Init(BackpackTag.packList);
        foreach (var item in backpackDic.Values)
        {
            packList.Add(item.GetString());
        }
        data.Add(packList);
        PlayerPrefs.SetString(BackpackTag.backpackData, data.GetString());*/
    }
    /// <summary>
    /// 重置基础属性 初始/等级改变
    /// </summary>
    public void ReLoadBaseAttribute()
    {
        var currentLvStr = lv.baseValue.ToString();
        var ad = JSONUserUpgrade.getInstance().GetAD(currentLvStr);
        var ap = JSONUserUpgrade.getInstance().GetAP(currentLvStr);
        var hp = JSONUserUpgrade.getInstance().GetHP(currentLvStr);
        var df = JSONUserUpgrade.getInstance().GetDF(currentLvStr);
        var pf = JSONUserUpgrade.getInstance().GetPF(currentLvStr);
        var crit = JSONUserUpgrade.getInstance().GetCrit(currentLvStr);
        var speed = JSONUserUpgrade.getInstance().GetSpeed(currentLvStr);
        var lucky = JSONUserUpgrade.getInstance().GetLucky(currentLvStr);
        attributeDic[enUserAttribute.ad].baseValue = ad;
        attributeDic[enUserAttribute.ap].baseValue = ap;
        attributeDic[enUserAttribute.hp].baseValue = hp;
        attributeDic[enUserAttribute.df].baseValue = df;
        attributeDic[enUserAttribute.pf].baseValue = pf;
        attributeDic[enUserAttribute.crit].baseValue = crit;
        attributeDic[enUserAttribute.speed].baseValue = speed;
        attributeDic[enUserAttribute.lucky].baseValue = lucky;
    }
    /// <summary>
    /// 重置装备对角色的属性加成
    /// </summary>
    void _ReLoadAdditionAttributeByEquip()
    {
        foreach (var e in equipments.Values)
        {
            if(e != null)
            {
                for(int i = 0 ; i < e.attributeData.attributes.Length; ++i)
                {
                    var type = (enUserAttribute)e.attributeData.attributes[i].key;
                    var value = e.attributeData.attributes[i].value;
                    attributeDic[type].additionValue += value;
                }
            }
        }
    }
    /// <summary>
    /// 获取各等级默认的技能列表
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    string[] _GetDefaultSkills(int lv)
    {
        if (_defaultSkillDic == null)
            _defaultSkillDic = new Dictionary<int, string[]>();
        if (_defaultSkillDic.ContainsKey(lv))
            return _defaultSkillDic[lv];
        else
        {
            var tempStr = JSONUserUpgrade.getInstance().GetSkill(lv.ToString());
            var tempNode = JSONNode.Parse(tempStr);
            var idArr = new string[tempNode.Count];
            for (int i = 0; i < tempNode.Count; ++i)
            {
                idArr[i] = tempNode[i];
            }
            _defaultSkillDic.Add(lv, idArr);
            return idArr;
        }
    }
    /// <summary>
    /// 根据等级/装备获取角色技能字典
    /// </summary>
    public void _ReLoadSkill()
    {
        skillDic.Clear();
        var idArr = _GetDefaultSkills(lv.baseValue);
        for(int i = 0; i < idArr.Length; ++i)
        {
            skillDic.Add(i, idArr[i]);
        }
        foreach(var equip in equipments.Values)
        {
            if(equip != null)
            {
                var skillData = equip.attributeData.carrySkill;
                if (skillData.isValid)
                {
                    skillDic[skillData.slotIndex] = skillData.skillID;
                }
            }
        }
    }
    /// <summary>
    /// 装备调整
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    public void ChangeEquip(enEquipmentType type ,  enItemData data)
    {
        bool equipChanged = false;
        bool skillChanged = false;
        if(data == null)
        {
            if (equipments.ContainsKey(type) && equipments[type] != null)
            {
                OnChangeEquip(equipments[type], false, out equipChanged);
                skillChanged = equipments[type].attributeData.carrySkill.isValid;
                equipments[type] = null;
            }
        }
        else
        {
            if (equipments.ContainsKey(type))
            {
                if (equipments[type] != null)
                    OnChangeEquip(equipments[type], false, out equipChanged);
                equipments[type] = data;
                OnChangeEquip(equipments[type], true, out equipChanged);
            }
            else
            {
                equipments.Add(type, data);
                OnChangeEquip(equipments[type], true, out equipChanged);
            }
        }
        if (equipChanged)
        {
            if (_onUserAttributeChange != null)
                _onUserAttributeChange();
            _ReLoadSkill();
            if (_onUserSkillChange != null)
                _onUserSkillChange();
        }
    }
    void OnChangeEquip(enItemData data , bool isPutOn , out bool isComplete)
    {
        var targetType = isPutOn ? 1 : -1;
        for(int i = 0 ; i < data.attributeData.attributes.Length; ++i)
        {
            var att = data.attributeData.attributes[i];
            attributeDic[(enUserAttribute)att.key].additionValue += att.value * targetType;
        }
        isComplete = true;
    }
}
public class MyInt
{
    System.Action<int> _onChangeCallback;
    int _baseValue;
    public int baseValue
    {
        get { return _baseValue; }
        set { _baseValue = value;
        if (_onChangeCallback != null)
            _onChangeCallback(_baseValue);
        }
    }
    int _additionValue;
    public int additionValue
    {
        get { return _additionValue; }
        set
        {
            _additionValue = value;
            if (_onChangeCallback != null)
                _onChangeCallback(_additionValue);
        }
    }
    public int Value
    {
        get { return baseValue + additionValue; }
    }
    public MyInt(int defaultValue = 0)
    {
        _baseValue = defaultValue;
    }
}
