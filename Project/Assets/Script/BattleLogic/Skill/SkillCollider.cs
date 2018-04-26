using UnityEngine;
using System.Collections;

public class SkillCollider : MonoBehaviour 
{
    bool _isActive;
    public bool isActive
    {
        get { return _isActive; }
        set { _isActive = value;}
    }
    ISingleSkill _rootSkill;
    protected ISingleSkill rootSkill
    {
        get {
            if (_rootSkill == null)
                _rootSkill = GetComponentInParent<ISingleSkill>();
            return _rootSkill;
        }
    }
    void OnEnable()
    {
        ///重置引用
        _rootSkill = null;
    }
    public void OnTriggerEnter(Collider other)
    {
        var actor = other.GetComponentInParent<ActorBase>();
        if(actor == null)
            return;

        rootSkill.OnCollided(actor , transform);
    }
    public void OnTriggerExit(Collider other)
    {
        var actor = other.GetComponentInParent<ActorBase>();
        if (actor == null)
            return;
        rootSkill.OnExitCollided(actor);
    }
}
