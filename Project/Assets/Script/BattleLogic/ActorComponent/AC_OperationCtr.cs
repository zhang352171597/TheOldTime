using UnityEngine;
using System.Collections;

/// <summary>
/// 摇杆操作控制器
/// </summary>
public class AC_OperationCtr : ActorComponentBase
{
    public override void OnAwake()
    {
        UIMgr.Instance.GetUI<BattleMainUI>().rockerModule.Bind(_onDirectionChange, _onOperationStop);

    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        
    }
	#region Callback
	void _onDirectionChange(Vector3 dir)
	{
        actor.Execute(enActorMessageType.tryMove);
		GetActorComponent<AC_MoveByDir>().Move(dir , Time.deltaTime);
	}
    void _onOperationStop()
    {
        actor.Execute(enActorMessageType.tryIdle);
    }
	#endregion

}
