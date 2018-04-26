using UnityEngine;
using System.Collections;
/// <summary>
/// 道具详情
/// </summary>
public class BattleItemInfoUI : UIBase 
{
    public BattleItemInfoModule currentInfo;
    public BattleItemInfoModule comparisonInfo;

    public void ResetData(enItemData currentData, enItemData ComparisonData)
    {
        currentInfo.ResetData(currentData);
        comparisonInfo.ResetData(ComparisonData);
    }
}
