using UnityEngine;
using System.Collections;

public class BattleSkillButtonCtr : MonoBehaviour 
{
    //长按多久显示详情
    float PRESSOFINFOTIME = 0.5f;

    public UISprite icon;
    public UISprite cdTime;
    public UISprite lockTip;
    public UILabel waitTime;
    public GameObject colliderObj;
    public UILabel infoLab;

    enSkillReleaseData _data;
    float _longTouchTimer;
    bool _showState;
    bool _canCreateSkill;

    void Awake()
    {
        UIEventListener.Get(colliderObj).onPress = _OnPress;
    }
    void _OnPress(GameObject go, bool state)
    {
        if (!state && !_showState && _canCreateSkill && _data != null)
            SkillMgr.Instance.CreateSkill(ActorMgr.Instance.player, _data.Release());
        _longTouchTimer = state ? PRESSOFINFOTIME : -1;
        _ChangeInfoDisplay(false);
    }
    void Update()
    {
        if(!_showState)
        {
            if (!_showState && _longTouchTimer > 0)
            {
                _longTouchTimer -= Time.deltaTime;
                if(_longTouchTimer <= 0)
                    _ChangeInfoDisplay(true);
            }
        }
    }
    void _ChangeInfoDisplay(bool state)
    {
        if (_showState != state)
        {
            if (state)
                infoLab.text = _data.currentStateData.singleData.info;
            _showState = state;
            infoLab.gameObject.SetActive(_showState);
        }
    }
	public void Init(enSkillReleaseData data)
    {
        _data = data;
        _data.Bind(CDPercentChange, beforeTimeChange, afterTimeChange, IconChange);
        _Reset();
    }
    void _Reset()
    {
        icon.spriteName = _data.currentStateData.singleData.icon;
        cdTime.fillAmount = 0;
        waitTime.text = "";
        _showState = true;
        lockTip.gameObject.SetActive(false);
        _canCreateSkill = true;
        _ChangeInfoDisplay(false);
    }
    public void IconChange(string iconName)
    {
        //TODO:图标置换特效
        icon.spriteName = iconName;
    }
    public void CDPercentChange(float percent)
    {
        cdTime.fillAmount = percent;
        icon.color = percent > 0 ? GameColor.Color_Grey : GameColor.Color_White;
        var targetColliderState = percent > 0 ? false : true;
        _canCreateSkill = targetColliderState; 
    }
    public void beforeTimeChange(float time)
    {
        if (time <= 0)
            waitTime.text = "";
        else
            waitTime.text = time.ToString("f1");
    }
    public void afterTimeChange(float time)
    {
        _canCreateSkill = time > 0 ? false : true;
    }
    public void SetLockState(bool state)
    {
        lockTip.gameObject.SetActive(state);
    }
}
