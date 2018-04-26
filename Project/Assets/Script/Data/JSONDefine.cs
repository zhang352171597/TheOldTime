using UnityEngine;
using System.Collections;

/// <summary>
/// JSON表通用标识
/// </summary>
public class JSONTag{

    public const string root = "Root";
    public const string id = "id";
    public const string name = "name";
    public const string icon = "icon";
    public const string info = "info";
    public const string key = "key";
    public const string value = "value";
}

/// <summary>
/// 技能标识
/// </summary>
public class SkillTag
{
    public const string JSONName = "JSONSkill";
    public const string coldTime = "coldTime";
    public const string readyTime = "readyTime";
    public const string beforeTime = "beforeTime";
    public const string afterTime = "afterTime";
    public const string states = "states";
    public const string prefabName = "prefabName";
    public const string bornPosOffset = "bornPosOffset";
}

/// <summary>
/// BUFF标识
/// </summary>
public class BuffTag
{
    public const string JSONName = "JSONBuff";
    public const string type = "type";
    public const string functionType = "functionType";
    public const string duration = "duration";
    public const string frequency = "frequency";
    public const string buffClips = "buffClips";
    public const string referType = "referType";
    public const string referTagType = "referTagType";
    public const string workType = "workType";
    public const string workTagType = "workTagType";
    public const string coreValueType = "coreValueType";
    public const string coreValue = "coreValue";
    public const string minValue = "minValue";
    public const string maxValue = "maxValue";
}

/// <summary>
/// 人物属性标识
/// </summary>
public class ActorAttributeTag
{
    public const string ad = "ad";
    public const string ap = "ap";
    public const string hp = "hp";
    public const string maxHp = "maxHp";
    public const string df = "df";
    public const string pf = "pf";
    public const string crit = "crit";
    public const string speed = "speed";
    public const string lucky = "lucky";
    public const string shield = "shield";
    /// <summary>
    /// 韧性 被控制时间按百分比削减
    /// </summary>
    public const string tenacity = "tenacity";
}

public class UserTag
{
    public const string userData = "userData";
    public const string equips = "equips";
}
/// <summary>
/// 背包标识
/// </summary>
public class BackpackTag
{
    public const string backpackData = "backpackData";
    public const string newItemUID = "newItemUID";
    public const string packList = "packList";
    public const string uid = "uid";
    public const string packIndex = "packIndex";
    public const string itemID = "itemID";
    public const string count = "count";
    public const string intensify = "intensify";
    public const string attributes = "attributes";
    public const string quality = "quality";
}
/// <summary>
/// 动画动作标识
/// </summary>
public class AnimatorTag
{
    public const string idle = "idle";
    public const string walk = "walk";
    public const string attack = "attack";
    public const string dead = "dead";
    public const string run = "run";
}
public class DropTag
{
    public const string mustDrop = "must";
    public const string randomDrop = "random";
}