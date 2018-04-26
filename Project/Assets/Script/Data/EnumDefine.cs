using UnityEngine;
using System.Collections;

/// <summary>
/// 阵营
/// </summary>
public enum enCamp
{
    /// <summary>
    /// 玩家
    /// </summary>
    player,
    /// <summary>
    /// 敌方
    /// </summary>
    enemy,
    /// <summary>
    /// 中立
    /// </summary>
    neutrality,
}

/// <summary>
/// 角色类型
/// </summary>
public enum enActorType
{
    /// <summary>
    /// 主角
    /// </summary>
    player = 0,
    /// <summary>
    /// 怪物
    /// </summary>
    monster,
    /// <summary>
    /// boss
    /// </summary>
    boss,
    /// <summary>
    /// 建筑
    /// </summary>
    build,
}

/// <summary>
/// 技能类型
/// </summary>
public enum enSkillType
{
    Base,
    /// <summary>
    /// 普通技能 --表现由子节点特殊表现控制器完成
    /// </summary>
    NormalSkill,
    /// <summary>
    /// buff技能 --开始添加buff  结束移除buff
    /// </summary>
    BuffSkill,
    /// <summary>
    /// 光环技能    --范围内添加buff 离开范围或者结束移除buff
    /// </summary>
    AureoleSkill,
    /// <summary>
    /// 跟随技能    --跟随释放者的技能
    /// </summary>
    FollowSkill,
}

/// <summary>
/// 限制状态类型
/// </summary>
public enum enLimitType
{
    NULL = 0,
    眩晕,
    击飞,
}

/// <summary>
/// 角色核心数据变更类型
/// </summary>
public enum enActorCoreAttChangeType
{
    基础变更 = 0,
    增益变更,
    减益变更
}

/// <summary>
/// buff类型
/// </summary>
public enum enBuffType
{
    /// <summary>
    /// 瞬间改变某条属性 结束buff
    /// </summary>
    瞬间 = 0,
    /// <summary>
    /// 某效果按频率持续作用
    /// </summary>
    持续,
    /// <summary>
    /// 改变某条属性  在持续时间结束 混滚该属性
    /// </summary>
    状态,
}

/// <summary>
/// buff取值参照方类型
/// </summary>
public enum enBuffReferType
{
    施法者 = 0,
    受击者,
}

/// <summary>
/// buff取值/作用数值标识
/// </summary>
public enum enBuffAttTargetTag
{
    无 = 0,
    最大血量 = 1,
    当前血量,
    已损血量,
    物攻,
    物防,
    魔攻,
    魔防,
    速度,
}
/// <summary>
/// buff功能类型
/// </summary>
public enum enBuffFunctionType
{
    基础 = 0,
    增益,
    减益,
}

/// <summary>
/// buff核心数值类型
/// </summary>
public enum enBuffCoreValueType
{
    固定值 = 0,
    百分比,
}

/// <summary>
/// 角色消息类型
/// </summary>
public enum enActorMessageType
{
    /// <summary>
    /// 位置更新
    /// </summary>
    onUpdatePos,
    /// <summary>
    /// 血量更新
    /// </summary>
    onUpdateHp,
    /// <summary>
    /// 血量上限更新
    /// </summary>
    onUpdatHPMax,
    /// <summary>
    /// 护盾更新
    /// </summary>
    onUpdateShield,
    /// <summary>
    /// 限制信息更新
    /// </summary>
    onUpdateLimitInfo,
    /// <summary>
    /// 获取当前角色状态  return INT 
    /// </summary>
    getCurrentState,
    /// <summary>
    /// 尝试掉落
    /// </summary>
    tryDropItem,
    /// <summary>
    /// 尝试死亡
    /// </summary>
    tryDead,
    /// <summary>
    /// 死亡
    /// </summary>
    onDead,
    /// <summary>
    /// 尝试位移
    /// </summary>
    tryMove,
    /// <summary>
    /// 确认位移
    /// </summary>
    onMove,
    /// <summary>
    /// 尝试待机
    /// </summary>
    tryIdle,
    /// <summary>
    /// 确认待机
    /// </summary>
    onIdle,
    /// <summary>
    /// 尝试销毁
    /// </summary>
    tryDestroy,
    /// <summary>
    /// 抖动相机 1~3对应三种抖动程度
    /// </summary>
    tryShakeCamera,
}

public enum enGameMessageType
{
    /// <summary>
    /// 世界相机更新
    /// </summary>
    worldCameraUpdate,
    /// <summary>
    /// 强制设置主角位置
    /// </summary>
    setPlayerPosForce,
}

public enum enItemGrade
{
    /// <summary>
    /// 普通
    /// </summary>
    Normal = 0,
    /// <summary>
    /// 稀有
    /// </summary>
    Uncommon,
    /// <summary>
    /// 卓越
    /// </summary>
    Excellence,
    /// <summary>
    /// 史诗
    /// </summary>
    Epic,
}

/// <summary>
/// 角色状态类型
/// </summary>
public enum enActorState
{
    /// <summary>
    /// 入场动作
    /// </summary>
    coming,
    /// <summary>
    /// 待机
    /// </summary>
    idle,
    /// <summary>
    /// 移动
    /// </summary>
    move,
    /// <summary>
    /// 死亡
    /// </summary>
    dead,
    /// <summary>
    /// 被限制--不可更改状态
    /// </summary>
    limited,
    /// <summary>
    /// 被击
    /// </summary>
    hurt,
}

public enum enItemType
{
    /// <summary>
    /// 货币
    /// </summary>
    curreny = 11,
    /// <summary>
    /// 材料
    /// </summary>
    material = 12,
    /// <summary>
    /// 消耗品
    /// </summary>
    consumables = 13,
    /// <summary>
    /// 装备
    /// </summary>
    equipment = 14,
}

public enum enEquipmentType
{
    /// <summary>
    /// 武器
    /// </summary>
    weapon = 1,
    /// <summary>
    /// 帽子
    /// </summary>
    cap = 2,
    /// <summary>
    /// 衣服
    /// </summary>
    clothes = 3,
    /// <summary>
    /// 护腕
    /// </summary>
    cuff = 4,
    /// <summary>
    /// 饰品
    /// </summary>
    accessory = 5,
    /// <summary>
    /// 鞋子
    /// </summary>
    shoes = 6,
    /// <summary>
    /// 披风
    /// </summary>
    cape = 7,
}

/// <summary>
/// 用户/装备属性类型
/// </summary>
public enum enUserAttribute
{
    unKnow = 0,
    /// <summary>
    /// 物攻
    /// </summary>
    ad,
    /// <summary>
    /// 魔攻
    /// </summary>
    ap,
    /// <summary>
    /// 血量
    /// </summary>
    hp,
    /// <summary>
    /// 物抗
    /// </summary>
    df,
    /// <summary>
    /// 魔抗
    /// </summary>
    pf,
    /// <summary>
    /// 暴击
    /// </summary>
    crit,
    /// <summary>
    /// 移速
    /// </summary>
    speed,
    /// <summary>
    /// 幸运
    /// </summary>
    lucky,
    Count,
}
/// <summary>
/// 技能-特效创建剪辑类型
/// </summary>
public enum enSkillTweenCreateType
{
    onStart,
    onFinish,
}
/// <summary>
/// 技能表现反馈于技能数据的类型
/// </summary>
public enum enSkillActionBackType
{
    none,
    onHited,
}