using UnityEngine;
using System.Collections;

public class TestOperation : MonoBehaviour
{
    BackpackMainUI a;
	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            var data = new enActorConstructionData();
            data.camp = enCamp.enemy;
            data.type = enActorType.monster;
            data.id = "14101";
            var tempMonster = ActorMgr.Instance.CreateActor(data);
            tempMonster.position = new Vector3(Random.Range(0,6), 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DataCenter.Instance.backpackData.CostTest();
        }

    }

}
