using UnityEngine;
using System.Collections;
using JSON;

public class test : MonoBehaviour 
{

    void Start()
    {
		DataCenter.Instance.backpackData.Add("140601", 1);
		DataCenter.Instance.backpackData.Add("142101", 1);
		DataCenter.Instance.backpackData.Add("143201", 1);
    }

    #region 测试类型
    /// <summary>
    /// 测试背包物品
    /// </summary>
    void _BackpackAndEquipment()
    {
        UIMgr.Instance.GetUI<BackpackMainUI>().Load();
        UIMgr.Instance.GetUI<BackpackMainUI>();
        UIMgr.Instance.GetUI<EquipmentMainUI>();
        DataCenter.Instance.backpackData.Add("12002", 99);
        DataCenter.Instance.backpackData.Add("12003", 99);
        DataCenter.Instance.backpackData.Add("12004", 99);
        DataCenter.Instance.backpackData.Add("142101", 2);
        DataCenter.Instance.backpackData.Add("142201", 1);
        DataCenter.Instance.backpackData.Add("142301", 1);
        DataCenter.Instance.backpackData.Add("142401", 1);
        DataCenter.Instance.backpackData.Add("142501", 1);
        DataCenter.Instance.backpackData.Add("142601", 1);
    }
    #endregion
    
}
