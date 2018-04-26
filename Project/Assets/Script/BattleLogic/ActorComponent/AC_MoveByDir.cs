using UnityEngine;
using System.Collections;

/// <summary>
/// 角色位移控制器
/// </summary>
public class AC_MoveByDir : ActorComponentBase 
{

	public void Move(Vector3 dir , float dt)
	{
		actor.forward = dir;
        var lastPos = actor.position;
        var targetPos = lastPos + actor.forward * actor.speed / 100 * dt;
        actor.position = Vector3.Slerp(lastPos, targetPos, 0.8f);
	}

}
