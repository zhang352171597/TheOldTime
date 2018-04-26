using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JSON;

public class BackpackData 
{
    //-------------LocalData-----------------
    /// <summary>
    /// 背包格子数量
    /// </summary>
    public const int GRIDCOUNT = 100;
    static int _newItemUID;
    /// <summary>
    /// 新建道具索引
    /// </summary>
    public static string newItemUID
    {
        get 
        {
            _newItemUID++;
            return "item_" + _newItemUID.ToString("000000");

        }
    }
    //---------------Control-------------------
    Dictionary<int, enItemData> _backpackDic;
    public Dictionary<int, enItemData> backpackDic
    {
        get
        {
            if (_backpackDic == null)
                _backpackDic = new Dictionary<int, enItemData>();
            return _backpackDic;
        }
    }
    Dictionary<int, enItemData> _changedCache;
    /// <summary>
    /// 背包变化缓存
    /// </summary>
    public Dictionary<int, enItemData> changedCache
    {
        get
        {
            if (_changedCache == null)
                _changedCache = new Dictionary<int, enItemData>();
            return _changedCache;
        }
    }
    System.Action<enItemData> _onChangeItemCallback;
    public System.Action<enItemData> onChangeItemCallback
    { get { return _onChangeItemCallback; }
      set { _onChangeItemCallback = value; }}
    System.Action<int> _onChangeSoulCallback;
    public System.Action<int> onChangeSoulCallback
    {get { return _onChangeSoulCallback; }
     set { _onChangeSoulCallback = value; }}
    System.Action<int> _onChangeExpCallback;
    public System.Action<int> onChangeExpCallback
    { get { return _onChangeExpCallback; }
    set { _onChangeExpCallback = value; }}

    int _soul;
    public int soul
    {get { return _soul; }
        set
        {
            _soul = value;
            if (onChangeExpCallback != null)
                onChangeExpCallback(_soul);
        }
    }
    int _exp;
    public int exp
    {
        get { return _exp; }
        set
        {
            _exp = value;
            if (onChangeExpCallback != null)
                onChangeExpCallback(_exp);
        }
    }
    public void Load()
    {
        backpackDic.Clear();
        var tempStr = PlayerPrefs.GetString(BackpackTag.backpackData , "");
        if(tempStr.Length > 0)
        {
            var rootNode = JSONNode.Parse(tempStr);
            _newItemUID = rootNode[BackpackTag.newItemUID].AsInt;
            var packNode = rootNode[BackpackTag.packList];
            for(int i = 0 ; i < packNode.Count; ++i)
            {
                var d = new enItemData(packNode[i]);
                backpackDic.Add(d.backpackIndex, d);
            }
        }
    }
    public void Save()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(BackpackTag.newItemUID, _newItemUID);
        ArrJsonData packList = ArrJsonData.Init(BackpackTag.packList);
        foreach(var item in backpackDic.Values)
        {
            packList.Add(item.GetString());
        }
        data.Add(packList);
        PlayerPrefs.SetString(BackpackTag.backpackData, data.GetString());
    }
    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="itemID"></param>
    /// <param name="count"></param>
    /// <returns>是否添加成功</returns>
    public bool Add(string itemID , int count)
    {
        if(itemID == "11001" || itemID == "12001")
        {
            var targetItem = itemID == "11001" ? "金币" : "经验值";
            UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.getProps, targetItem + " x"+ count);
            if (itemID == "11001")
                soul += count;
            else if (itemID == "12001" && _onChangeExpCallback != null)
                exp += count;
            return true;
        }
        else
        {
            var superposition = JSONItem.getInstance().GetSuperposition(itemID) == 1;
            if (superposition)
            {
                foreach (var d in backpackDic.Values)
                {
                    if (d.id == itemID)
                    {
                        d.count += count;
                        _OnItemChange(d);
                        UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.getProps, d.nameWithColor + " x" + count);
                        return true;
                    }
                }
                return _Add(itemID, count);
            }
            else
            {
                for (int i = 0; i < count; ++i)
                {
                    var result = _Add(itemID, 1);
                    if (!result)
                        return false;
                }
                return true;
            }
        }
    }
    bool _Add(string itemID , int count)
    {
        for(int index = 0; index < GRIDCOUNT; ++index)
        {
            if (!backpackDic.ContainsKey(index))
            {
                var d = new enItemData(itemID, index);
                d.count = count;
                backpackDic.Add(index, d);
                _OnItemChange(d);
                UIMgr.Instance.GetUI<HintUI>().ShowHint(HintUI.eHintType.getProps, d.nameWithColor + " x" + count);
                return true;
            }
        }
        ///背包已满 无法拾取
        return false;
    }

    /// <summary>
    /// 当道具状态发生改变
    /// </summary>
    /// <param name="data"></param>
    void _OnItemChange(enItemData data)
    {
        if (onChangeItemCallback == null)
        {
            if (changedCache.ContainsKey(data.backpackIndex))
                changedCache[data.backpackIndex] = data;
            else
                changedCache.Add(data.backpackIndex, data);
        }
        else
            onChangeItemCallback(data);
    }
    /// <summary>
    /// 消耗道具
    /// </summary>
    /// <param name="packIndex"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool Cost(int packIndex , int count)
    {
        if(backpackDic.ContainsKey(packIndex))
        {
            var d = backpackDic[packIndex];
            if (d.count >= count)
            {
                d.count -= count;
                _OnItemChange(d);
                if (d.count == 0)
                    backpackDic.Remove(packIndex);
            }
            else
                return false;
        }
        return false;
    }
    public void CostTest()
    {
        foreach(var d in backpackDic.Values)
        {
            Cost(d.backpackIndex, 1);
            return;
        }
    }
}

public class enItemData
{
    /// <summary>
    /// 物品背包索引
    /// </summary>
    int _backpackIndex;
    public int backpackIndex
    {
        get { return _backpackIndex; }
        set { _backpackIndex = value; }
    }
    /// <summary>
    /// 物品UID
    /// </summary>
    string _itemUID;
    public string itemUID
    { get { return _itemUID; } }
    /// <summary>
    /// 物品ID
    /// </summary>
    string _id;
    public string id
    { get { return _id; } }
    /// <summary>
    /// 物品数量
    /// </summary>
    int _count;
    public int count
    {
        get { return _count; }
        set { _count = value; }
    }
    enEquipmentAttributes _attributeData;
    public enEquipmentAttributes attributeData
    { get { return _attributeData; } }
    //--------FromLocal------------
    int _firstType;
    /// <summary>
    /// 第一类型
    /// </summary>
    public enItemType firstType
    {
        get { return (enItemType)_firstType; }
    }
    int _lastType;
    /// <summary>
    /// 第二类型
    /// </summary>
    public int lastType
    {
        get { return _lastType; }
    }
    bool _superposition;
    /// <summary>
    /// 可否叠加
    /// </summary>
    public bool superposition
    { get { return _superposition; } }

    string _info;
    public string info
    { get { return _info; } }
    string _icon;
    public string icon
    { get { return _icon; } }
    string _name;
    public string name
    { get { return _name; } }
    string _nameWithColor;
    public string nameWithColor
    { get { return _nameWithColor; } }
    Color _qualityColor;
    public Color qualityColor
    { get { return _qualityColor; } }
    int _quality;
    public int quality
    { get { return _quality; } }
    string _qualityWithColor;
    public string qualityWithColor
    { get { return _qualityWithColor; } }
    /// <summary>
    /// 创建物品数据 传入ID  背包索引
    /// </summary>
    /// <param name="id"></param>
    /// <param name="index"></param>
    public enItemData(string id , int index)
    {
        _itemUID = BackpackData.newItemUID;
        _backpackIndex = index;
        _id = id;
        _count = 1;
        _ReadLocal();
        _MakeAttribute();
    }
    public enItemData(JSONNode node)
    {
        _id = node[BackpackTag.itemID];
        _itemUID = node[BackpackTag.uid];
        _backpackIndex = node[BackpackTag.packIndex].AsInt;
        _count = node[BackpackTag.count].AsInt;
        var attributeNode = node[BackpackTag.attributes];
        if (attributeNode.ToString().Length != 0)
            _attributeData = new enEquipmentAttributes(attributeNode);
        _ReadLocal();
    }
    public string GetString()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(BackpackTag.itemID, _id);
        data.Add(BackpackTag.uid, _itemUID);
        data.Add(BackpackTag.packIndex, _backpackIndex);
        data.Add(BackpackTag.count, _count);
        if (_attributeData != null)
            data.Add(BackpackTag.attributes, _attributeData.GetString());
        return data.GetString();
    }
    void _MakeAttribute()
    {
        if (firstType == enItemType.equipment)
            _attributeData = new enEquipmentAttributes(id);
    }
    void _ReadLocal()
    {
        var typeValue = JSONItem.getInstance().GetType(_id);
        _firstType = typeValue / 100;
        _lastType = typeValue % 100;
        _superposition = JSONItem.getInstance().GetSuperposition(_id) == 1;
        _info = JSONItem.getInstance().GetInfo(_id);
        _icon = "" + _id;
        _name = JSONItem.getInstance().GetName(id);
        _quality = JSONItem.getInstance().GetQuality(id);
        _qualityColor = Contrastting.qualityColor[_quality];
        _qualityWithColor = Contrastting.qualityOfText[_quality] + Contrastting.qualityNames[_quality] + Contrastting.qualityOfText_Normal;
        _nameWithColor = Contrastting.qualityOfText[_quality] + _name + Contrastting.qualityOfText_Normal;
    }
}

public class enEquipmentAttributes
{
    public struct strAttribute
    {
        public int key;
        public int value;
        //属性品阶
        public int quality;
    }

    public struct strCarrySkill
    {
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool isValid;
        /// <summary>
        /// 技能站位
        /// </summary>
        public int slotIndex;
        public string skillID;
        public string skillInfo;
    }

    /// <summary>
    /// 属性池
    /// </summary>
    strAttribute[] _attributes;
    public strAttribute[] attributes
    { get { return _attributes; } }

    int _intensify;
    /// <summary>
    /// 强化数值
    /// </summary>
    public int intensify
    {get { return _intensify; }}

    strCarrySkill _carrySkill;
    /// <summary>
    /// 装备上携带的技能
    /// </summary>
    public strCarrySkill carrySkill
    { get { return _carrySkill; } }

    public enEquipmentAttributes(string id)
    {
        _intensify = 0;
        _MakeEquipAttribute(id);
        _CheckCarrySkill(id);
    }
    public enEquipmentAttributes(string id , JSONNode node)
    {
        _intensify = node[BackpackTag.intensify].AsInt;
        var attNode = node[BackpackTag.attributes];
        _attributes = new strAttribute[attNode.Count];
        for(int i = 0 ; i < attNode.Count; ++i)
        {
            strAttribute attData;
            attData.key = attNode[i][JSONTag.key].AsInt;
            attData.value = attNode[i][JSONTag.value].AsInt;
            attData.quality = attNode[i][BackpackTag.quality].AsInt;
            attributes[i] = attData;
        }
        _CheckCarrySkill(id);
    }
    public string GetString()
    {
        var data = ListJsonData.Init();
        data.Add(BackpackTag.intensify, _intensify);
        var arr = ArrJsonData.Init(BackpackTag.attributes);
        for (int i = 0; i < attributes.Length; ++i)
        {
            var attData = ListJsonData.Init();
            attData.Add(JSONTag.key , attributes[i].key);
            attData.Add(JSONTag.value , attributes[i].value);
            attData.Add(BackpackTag.quality, attributes[i].quality);
            arr.Add(attData);
        }
        data.Add(arr);
        return data.GetString();
    }
    void _MakeEquipAttribute(string itemID)
    {
        var attStr = JSONEquip.getInstance().GetAttribute(itemID);
        var dict = JSONNode.Parse(attStr).AsObject.m_Dict;
        _attributes = new strAttribute[dict.Count];
        var index = 0;
        foreach(var v in dict)
        {
            _attributes[index].key = int.Parse(v.Key);
            try
            {
                var tempRandom = v.Value.ToString().Split('-');
                var valueMin = float.Parse(tempRandom[0]);
                var valueMax = float.Parse(tempRandom[1]);
                _attributes[index].quality = Random.Range(0, 5);
                _attributes[index].value = (int)(valueMin + (valueMax - valueMin) * _attributes[index].quality / 4);
            }
            catch
            {
                EditorDebug.LogError("初始装备属性，装备ID" + itemID);
            }
            index++;
        }
    }
    void _CheckCarrySkill(string id)
    {
        var tempStr = JSONEquip.getInstance().GetSkill(id);
        if (tempStr.Length > 0)
        {
            _carrySkill.isValid = true;
            var node = JSONNode.Parse(tempStr);
            _carrySkill.slotIndex = node[0].AsInt;
            _carrySkill.skillID = node[1];
            _carrySkill.skillInfo = node[2];
        }
    }
}
