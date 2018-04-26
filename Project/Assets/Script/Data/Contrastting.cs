using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Contrastting
{
    /// <summary>
    /// 品阶名称对照表
    /// </summary>
    public static string[] qualityNames = { "普通", "精良" ,"稀有", "史诗", "传说" };
    static Color[] _qualityColor;
    /// <summary>
    /// 品阶颜色对照表
    /// </summary>
    public static Color[] qualityColor
    {
        get
        {
            if (_qualityColor == null)
                _qualityColor = new Color[] { Color.white, Color.green, Color.blue, new Color(1, 0, 1), new Color(1, 0.6f, 0) };
            return _qualityColor;
        }
    }
    static string[] _qualityOfText;
    /// <summary>
    /// 品阶颜色字符串对照表
    /// </summary>
    public static string[] qualityOfText
    {
        get
        {
            if (_qualityOfText == null)
                _qualityOfText = new string[] {"[999999]", "[00FF16FF]", "[3300FF]", "[660099]", "[FF6600]"};
            return _qualityOfText;
        }
    }

    static string[] _equipAttrbuteName;
    public static string[] equipAttrbuteName
    {
        get
        {
            if (_equipAttrbuteName == null)
                _equipAttrbuteName = new string[] { "未知", "物攻", "魔攻", "血量", "物抗", "魔抗", "暴击", "移速" ,"幸运"};
            return _equipAttrbuteName;
        }
    }

    public static string qualityOfText_Normal = "[-]";

    static Dictionary<enEquipmentType, string> _equipmentName;
    public static string GetEquipmentName(enEquipmentType type)
    {
        if (_equipmentName == null)
        {
            _equipmentName = new Dictionary<enEquipmentType, string>();
            _equipmentName.Add(enEquipmentType.accessory, "饰品");
            _equipmentName.Add(enEquipmentType.cap, "帽子");
            _equipmentName.Add(enEquipmentType.cape, "披风");
            _equipmentName.Add(enEquipmentType.clothes, "衣服");
            _equipmentName.Add(enEquipmentType.cuff, "护腕");
            _equipmentName.Add(enEquipmentType.shoes, "鞋子");
            _equipmentName.Add(enEquipmentType.weapon, "武器");
        }
        if (_equipmentName.ContainsKey(type))
            return _equipmentName[type];
        else
            return "？？？";
    }

}
