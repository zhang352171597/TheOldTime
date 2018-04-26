using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AC_ShaderCtr : ActorComponentBase
{
    TimerCtr _timer;
    Shader _defaultShader;
    Shader defaultShader
    {
        get
        {
            if (_defaultShader == null)
                _defaultShader = Shader.Find("Legacy Shaders/Diffuse");
            return _defaultShader;
        }
    }
    Shader _deadShader;
    Shader deadShader
    {
        get
        {
            if (_deadShader == null)
                _deadShader = Shader.Find("MyShader/FXDissolve");
            return _deadShader;
        }
    }
    List<Material> _matsCache;
    float _deadCounter;
    bool _onDead;
    Texture _mainTex;
    bool _isInited;

    public override void OnAwake()
    {
        _timer = new TimerCtr();
        _onDead = false;
        actor.Bind(enActorMessageType.onDead, _OnDead);
        actor.Bind(enActorMessageType.tryDestroy, _OnTryDestroy);
    }
    public override void OnDestroy()
    {
        actor.UnBind(enActorMessageType.onDead, _OnDead);
        actor.UnBind(enActorMessageType.tryDestroy, _OnTryDestroy);
    }
    public override void LogicUpdate(float dt)
    {
        _timer.UpdateLogic(dt);
        _OnShaderChange(dt);
    }
    #region Callback
    void _OnDead(params System.Object[] objs)
    {
         _timer.AddTimerEvent(_ToChange, 1.5f);
    }
    void _OnTryDestroy(params System.Object[] objs)
    {
        _CheckIsDefaultShader();
    }
    #endregion
    void _CheckInit()
    {
        if (_isInited)
            return;
        _isInited = true;
    }
    void _CheckIsDefaultShader()
    {
        var modelCtr = actor.GetActorComponent<AC_ModelCtr>();
        modelCtr.SetShader(defaultShader);
    }
    void _ToChange()
    {
        var modelCtr = actor.GetActorComponent<AC_ModelCtr>();
        _matsCache = modelCtr.SetShader(deadShader);
        for (int i = 0; i < _matsCache.Count; ++i )
        {
            if (_matsCache[i].HasProperty("_tietu"))
                _matsCache[i].SetTexture("_tietu", modelCtr.mainTex);
        }
        _deadCounter = 1;
        _onDead = true;
    }
    void _OnShaderChange(float dt)
    {
        if (_onDead && _deadCounter > 0.5f)
        {
            _deadCounter -= dt * 0.5f;
            for(int i = 0 ; i < _matsCache.Count; ++i)
            {
                if(_matsCache[i].HasProperty("_rongjie"))
                    _matsCache[i].SetFloat("_rongjie", _deadCounter);
            }
        }
    }
}
