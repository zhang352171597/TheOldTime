using UnityEngine;
using System.Collections;
using System;

public class LoadingUI : UIBase
{
	static string[] tipArr = { "具备特技的装备，可以替换掉你的基础技能", "同样的装备也有属性优劣之分", "办证 15710689317", "这个开发者很懒，什么都没有留下" };
	static int tipIndex = 0;
    const float LABOFFSETMAX = 685;
    public UISprite progress;
    public UILabel lab;
	public UILabel tip;
    float targetPercent;
    float _currentPercent;
    Vector3 _labOffsetV;
    Action _completeBack;
    Animator _ani;
    Animator ani
    {
        get
        {
            if (_ani == null)
                _ani = GetComponentInChildren<Animator>();
            return _ani;
        }    
    }
    void OnEnable()
    {
        _Reset();
        ani.SetTrigger(AnimatorTag.run);
    }
    public void Begin(IEnumerator i , Action completeBack)
    {
        _currentPercent = 0;
        _completeBack = completeBack;
        StartCoroutine(i);
    }
	public void UpdateProgress(float percent)
    {
        targetPercent = percent;
    }
    void _Reset()
    {
        _currentPercent = 1;
        targetPercent = 0;
        _labOffsetV.x = 0;
        lab.transform.parent.localPosition = _labOffsetV;
        lab.text = "0%";
        progress.fillAmount = 0;
		tip.text = "Tip：" + tipArr [tipIndex];
		tipIndex++;
		if (tipIndex >= tipArr.Length)
			tipIndex = 0;
    }
    void _OnComplete()
    {
        Invoke("_Callback" , 0.5f);
    }
    void _Callback()
    {
        if (_completeBack != null)
            _completeBack();
    }
    void Update()
    {
        var dt = Time.deltaTime;
        if(_currentPercent < targetPercent)
        {
            _currentPercent += dt;
            if(_currentPercent >= 1)
            {
                _currentPercent = 1;
                _OnComplete();
            }
            _labOffsetV.x = LABOFFSETMAX * _currentPercent;
            lab.transform.parent.localPosition = _labOffsetV;
            lab.text = (int)(_currentPercent * 100) + "%";
			progress.fillAmount = _currentPercent;
        }
    }

}
