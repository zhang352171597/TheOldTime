using UnityEngine;
using System.Collections;

public class SkillTweenRotBySelf : SkillTweenBase
{
	public float rotSpeed;
	float _currentRot;
	public override void Play()
	{
		base.Play ();
		_currentRot = 0;
	}

	public override void UpdateLogic(float dt)
	{
		base.UpdateLogic(dt);
		_RootUpdate (dt);
	}
	void _RootUpdate(float dt)
	{
		_currentRot += dt * rotSpeed;
		transform.RotateAround (transform.position, Vector3.up, _currentRot);
	}
}
