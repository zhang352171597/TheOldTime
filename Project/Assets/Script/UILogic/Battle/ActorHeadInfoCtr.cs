using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorHeadInfoCtr : MonoBehaviour 
{
    public UISprite hpBar;
    public UILabel hpNumLab;
    public UISprite shieldBar;
    public UILabel limitTip;
    public UISprite limitBar;
    public Transform NumbericGroupTrm;

    enCamp _camp;
    public enCamp camp
    {
        get { return _camp; }
    }

    enLimitType _currentLimit;

    int _currentCutNumCache;
    int _currentAddNumCache;

    public void Begin(enCamp c , Vector3 pos)
    {
        _camp = c;
        _UpdateColor();
        UpdatePos(pos);
    }

    public void UpdateLimitPercent(enLimitType type , float percent)
    {
        if(_currentLimit != type)
        {
            limitTip.gameObject.SetActive(true);
            limitTip.text = type.ToString();
            _currentLimit = type;
        }
        limitBar.fillAmount = percent;
        if(percent <= 0)
        {
            limitTip.gameObject.SetActive(false);
            _currentLimit = enLimitType.NULL;
        }
    }

    public void UpdateHp(float current , float max)
    {
        hpBar.fillAmount = (float)current / max;
        hpNumLab.text = current + " / " + max;
    }

    public void UpdateShield(float percent)
    {
        shieldBar.fillAmount = percent;
    }

    void _ShowNumber(ref int value)
    {
        if (value == 0)
            return;
        //TODO:预加载
        var obj = ObjManager.Instance.addChild(GamePath.UIPrefabs + "Tools/", "ActorHeadInfo_Num", NumbericGroupTrm , true);
        var spawnCtr = obj.GetComponent<DespawnByTime>();
        if (spawnCtr == null)
            spawnCtr = obj.AddComponent<DespawnByTime>();
        spawnCtr.Begin(2);
        obj.transform.localPosition = Vector3.up * Random.Range(0, 6) * 5;
        var temp = obj.GetComponentInChildren<UILabel>(true);
        temp.color = value >= 0 ? Color.green : Color.red;
        var tempStr = value >= 0 ? "+" : "";
        temp.text = tempStr + value;
        value = 0;
    }

    public void ShowNumber(int value)
    {
        if (value > 0)
            _currentAddNumCache += value;
        else if (value < 0)
            _currentCutNumCache += value;
    }

    public void UpdatePos(Vector3 pos)
    {
        transform.position = CalculateCenter.Pos_WorldToNGUI(pos);
    }

    void _UpdateColor()
    {
        switch(_camp)
        {
            case enCamp.player:
                hpBar.spriteName = "HealthBar";
                break;
            case enCamp.enemy:
                hpBar.spriteName = "AttackBar";
                break;
            case enCamp.neutrality:
                hpBar.spriteName = "DifficultyBar";
                break;
        }
    }

    void Update()
    {
        if(Time.frameCount % 10 == 0)
        {
            _ShowNumber(ref _currentAddNumCache);
            _ShowNumber(ref _currentCutNumCache);
        }
    }
}
