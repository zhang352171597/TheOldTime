using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色模型/动作控制器
/// </summary>
public class AC_ModelCtr : ActorComponentBase
{
    GameObject _model;
    Animator _ani;
    string _currentAniState;
    List<Material> _mats;
    Texture _mainTex;
    public Texture mainTex
    { get { return _mainTex; } }
    public override void OnAwake()
    {
        actor.gameObject.name = actor.camp + "_"+ actor.type + "_" + actor.id;
        var modelName = JSONModel.getInstance().GetMapID(actor.id);
        var scale = JSONModel.getInstance().GetScale(actor.id);
        _model = ObjManager.Instance.addChild(GamePath.ActorPrefabs, modelName, actor.transform);
        _model.transform.localScale = Vector3.one * scale;
        _model.transform.localRotation = Quaternion.Euler(Vector3.zero);
        _ani = _model.GetComponentInChildren<Animator>();
        _currentAniState = AnimatorTag.idle;
        actor.Bind(enActorMessageType.onMove, _OnMove);
        actor.Bind(enActorMessageType.onIdle, _OnIdle);
        actor.Bind(enActorMessageType.onDead, _OnDead);
        actor.Bind(enActorMessageType.tryDestroy, _TryDestroy);
        _CheckMats();
    }

    public override void OnDestroy()
    {
        actor.UnBind(enActorMessageType.onMove, _OnMove);
        actor.UnBind(enActorMessageType.onIdle, _OnIdle);
        actor.UnBind(enActorMessageType.onDead, _OnDead);
        actor.UnBind(enActorMessageType.tryDestroy, _TryDestroy);
    }
    #region Callback
    void _OnMove(params System.Object[] objs)
    {
        if (_currentAniState != AnimatorTag.run)
        {
            _currentAniState = AnimatorTag.run;
            _ani.Play(_currentAniState);
        }
    }

    void _OnIdle(params System.Object[] objs)
    {
        if (_currentAniState != AnimatorTag.idle)
        {
            _currentAniState = AnimatorTag.idle;
            _ani.Play(_currentAniState);
        }
    }
    void _OnDead(params System.Object[] objs)
    {
        if (_currentAniState != AnimatorTag.dead)
        {
            _currentAniState = AnimatorTag.dead;
            _ani.Play(_currentAniState);
        }
    }
    void _TryDestroy(params System.Object[] objs)
    {
        ObjManager.Instance.Despawn(_model);
    }
    #endregion

    void _CheckMats()
    {
        _mats = new List<Material>();
        var renderers = _model.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            var tempArr = renderers[i].materials;
            for (int j = 0; j < tempArr.Length; ++j)
            {
                _mats.Add(tempArr[j]);
            }
        }
        _mainTex = renderers[0].material.GetTexture("_MainTex");
    }
    public List<Material> SetShader(Shader shader)
    {
        for(int i = 0 ; i < _mats.Count; ++i)
        {
            _mats[i].shader = shader;
        }
        return _mats;
    }
}