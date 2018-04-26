using UnityEngine;
using System.Collections;

public class SkillTweenWithFall : SkillTweenBase
{
	/// <summary>
	/// 最小攻击距离 
	/// </summary>
    public float AttackDis;
	/// <summary>
	/// 最大攻击距离
	/// </summary>
    public float maxAttackDis;
    /// <summary>
    /// 高度
	/// </summary>
	public float height;
    /// <summary>
    /// 落点X轴偏移
	/// </summary>
	public Vector2 offsetX;
    /// <summary>
    /// 落点Z轴偏移
	/// </summary>
	public Vector2 offsetZ;
    /// <summary>
    /// 起始速度
	/// </summary>
	public float startSpeed;
    /// <summary>
    /// 下落加速度
	/// </summary>
	public float speedAddition;

    Vector3 _targetPos;
    Vector3 _fallDir;
    float _currentSpeed;
    bool _isFinished;
    public override void Play()
    {
        base.Play();
        var beginPos = Vector3.zero;
        RaycastHit hitinfo;
        var startPos = attacker.position + attacker.bodyOffset;
		if (Physics.Raycast (startPos, attacker.forward, out hitinfo, maxAttackDis)) {
			var obj = hitinfo.collider.gameObject.GetComponentInParent<ActorBase> ();
			if(obj == null)
				beginPos = position + attacker.forward * AttackDis;
			else
				beginPos = new Vector3(obj.position.x, 0, obj.position.z);
		}  
        else
            beginPos = position + attacker.forward * AttackDis;
        _targetPos = beginPos;
        _targetPos.y = 0.5f;
        position = new Vector3(beginPos.x + Random.Range(offsetX.x, offsetX.y), beginPos.y + height, beginPos.z + Random.Range(offsetZ.x, offsetZ.y));
        _fallDir = (_targetPos - position).normalized;
        _currentSpeed = startSpeed;
        _isFinished = false;
    }
    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        if (!_isFinished)
            _MoveUpdate(dt);
    }
    void _MoveUpdate(float dt)
    {
        _currentSpeed += speedAddition * dt;
        var tempDis = _currentSpeed * dt;
        var nextPos = position + _fallDir * tempDis;
        if (Vector3.Distance(position, _targetPos) <= tempDis || nextPos == position)
        {
            position = _targetPos;
            _isFinished = true;
            _FinishRoot();
        }
        else
            position = nextPos;
    }
}
