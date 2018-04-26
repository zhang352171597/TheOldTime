using UnityEngine;
using System.Collections;


public class ParticleData
{
    public float startSize;
}

/// <summary>
/// 粒子特效缩放器
/// </summary>
public class ParticleSystemCtr : MonoBehaviour {

    ParticleSystem[] _particle;
    ParticleData[] partilesCahedData;
    /// <summary>
    /// 是否已经初始化
    /// </summary>
    bool _inited;

    public bool isTest;
    public float targetScale;
    
    void Awake()
    {
        if (isTest)
            ResetScale(targetScale);
    }


    void _CheackInit()
    {
        if (_inited)
            return;

        _particle = GetComponentsInChildren<ParticleSystem>();
        if (_particle != null)
        {
            partilesCahedData = new ParticleData[_particle.Length];
            for (int i = 0; i < _particle.Length; i++)
            {
                partilesCahedData[i] = new ParticleData();
                partilesCahedData[i].startSize = _particle[i].startSize;
            }
        }
        _inited = true;
    }

    public void ResetScale(float size)
    {
        _CheackInit();
        transform.localScale = size * Vector3.one;
        if (_particle != null)
        {
            for (int i = 0; i < _particle.Length; i++)
            {
                _particle[i].startSize = partilesCahedData[i].startSize * size;
            }
        }
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

	public void OpenFlash(){
		_CheackInit ();
		if (_particle != null)
		{
			for (int i = 0; i < _particle.Length; i++)
			{
				var sizeOverLifeTime = _particle [i].sizeOverLifetime;
				var colorOverLifeTime = _particle [i].colorOverLifetime;
				sizeOverLifeTime.enabled = true;
				colorOverLifeTime.enabled = true;
			}
		}
	}
	public void HideFlash(){
		_CheackInit ();
		if (_particle != null)
		{
			for (int i = 0; i < _particle.Length; i++)
			{
				var sizeOverLifeTime = _particle [i].sizeOverLifetime;
				var colorOverLifeTime = _particle [i].colorOverLifetime;
				sizeOverLifeTime.enabled = false;
				colorOverLifeTime.enabled = false;
			}
		}
	}
}
