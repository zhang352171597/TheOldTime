using UnityEngine;
using System.Collections;

public class EffectRenderSetting : MonoBehaviour 
{
    public enum DepthMode
    {
        renderQueue,
        SortingOrder,
    }

    [Header("更新")]
    public bool _Update;
    public int _depth = 3000;
    public DepthMode depthMode = DepthMode.renderQueue;
    Renderer[] _render;
    ParticleRenderer[] _effectrender;
    bool _isInited;


	void Start () 
    {
        if (!_isInited)
        {
            _render = GetComponentsInChildren<Renderer>();
            _effectrender = GetComponentsInChildren<ParticleRenderer>();
            _UpdateRender();
        }
	}

    void Update()
    {
        if (_Update)
        {
            _Update = false;
            _UpdateRender();
        }
    }

    void _UpdateRender()
    {
        if (_render != null)
        {
            for (int i = 0; i < _render.Length; i++)
            {
                SetRenderDepth(_render[i], _depth);
            }
        }
        if (_effectrender != null)
        {
            for (int i = 0; i < _effectrender.Length; i++)
            {
                SetRenderDepth(_effectrender[i], _depth);
            }
        }
    }

    void SetRenderDepth(Renderer render, int value)
    {
        if (depthMode == DepthMode.renderQueue)
            render.material.renderQueue = _depth;
        else
            render.sortingOrder = value;
    }

}
