using UnityEngine;
using System.Collections;

public class BattleMapModule : MonoBehaviour {

    public UILabel mapName;

    public void ResetData(MapCtr map)
    {
        mapName.text = map.mapName;
    }

}
