using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 发射器
/// </summary>
public class SkillTweenGun : SkillTweenBase 
{
    /// <summary>
    /// 是否穿透
    /// </summary>
    public bool isRift;
    /// <summary>
    /// 频率
    /// </summary>
    public float frequency;
    /// <summary>
    /// 次数
    /// </summary>
    public int fireCount;
    /// <summary>
    /// 发射口角度范围
    /// </summary>
    public Vector2 rotRange;
    /// <summary>
    /// 间隔角度
    /// </summary>
    public float singleRot = 1;
    /// <summary>
    /// 子弹
    /// </summary>
    public GameObject bullet;
    /// <summary>
    /// 弹道速度
    /// </summary>
    public float bulletSpeed;
    /// <summary>
    /// 子弹池
    /// </summary>
    List<Transform> _bulletPool;
    /// <summary>
    /// 当前攻击次数
    /// </summary>
    int _currentFireCount;
    /// <summary>
    /// 默认朝向
    /// </summary>
    Vector3 _currentForward;


    public override void Play()
    {
        base.Play();
        _bulletPool = new List<Transform>();
        _currentForward = rootSkill.data.attacker.forward;
        _currentFireCount = 0;
        _Fire();
    }

    public override void UpdateLogic(float dt)
    {
        base.UpdateLogic(dt);
        _UpdateBullet(dt);
    }

    public override void OnCollided(Transform colliderTrm)
    {
        base.OnCollided(colliderTrm);
        if (!isRift)
        {
            ObjManager.Instance.Despawn(colliderTrm.gameObject);
            if (_bulletPool.Contains(colliderTrm))
                _bulletPool.Remove(colliderTrm);
            ///如果不穿透切所有技能
            if (_bulletPool.Count == 0)
                _FinishRoot();
        }
    }
    public override void OnFinish()
    {
        base.OnFinish();
        for(int i = 0; i < _bulletPool.Count; ++i)
        {
            ObjManager.Instance.Despawn(_bulletPool[i].gameObject);
        }
        _bulletPool.Clear();
    }
    void _Fire()
    {
        _currentFireCount++;
        if(rotRange == Vector2.zero )
            _CreateBullet(0);
        else
        {
            for (float tempRot = rotRange.x; tempRot <= rotRange.y; tempRot += singleRot)
            {
                _CreateBullet(tempRot);
            }
        }
        if (_currentFireCount < fireCount)
            AddTimerEvent(_Fire, frequency);
    }
    void _CreateBullet(float tempRot)
    {
        Vector3 tempV = new Vector3(0, tempRot, 0);
        var tempDir = Quaternion.Euler(tempV) * _currentForward;
        var obj = ObjManager.Instance.addChild(bullet, transform);
        obj.transform.LookAt(obj.transform.position + tempDir);
        _bulletPool.Add(obj.transform);
    }
    void _UpdateBullet(float dt)
    {
        for (int i = 0; i < _bulletPool.Count; ++i)
        {
            var targetPos = _bulletPool[i].position + _bulletPool[i].forward * bulletSpeed * dt;
            _bulletPool[i].position = Vector3.Slerp(_bulletPool[i].position , targetPos , 0.7f);
        }
    }
}
