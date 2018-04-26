using UnityEngine;
using System.Collections;

public class TransferCtr : MonoBehaviour
{
    [Header("目标地图ID")]
    public string mapID;
    public Vector3 standPos
    { get { return _standPosTrm.position; } }

    Transform _standPosTrm;
    GameObject _effectActive;
    TextMesh _targetName;

    const float maxWaitTime = 0.5f;
    float _waitTimer;
    bool _collidedWithPlayer;

    public void Begin()
    {
        enabled = true;
        _collidedWithPlayer = false;
        _waitTimer = 0;
        _standPosTrm = transform.FindChild("standPos");
        _effectActive = transform.FindChild("effectActive").gameObject;
        _targetName = GetComponentInChildren<TextMesh>();
        if (_targetName != null)
            _targetName.text = JSONMap.getInstance().GetName(mapID);
    }

    void Update()
    {
        if (!_collidedWithPlayer)
            return;
        _waitTimer += Time.deltaTime;
        if (_waitTimer > maxWaitTime)
            _Transfer();
    }

	void OnTriggerEnter(Collider other)
    {
        var playerCtr = other.GetComponentInParent<PlayerBase>();
        if (playerCtr != null)
        {
            _collidedWithPlayer = true;
            _waitTimer = 0;
            _effectActive.SetActive(true);
            Invoke("_HideEffect", 1);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var playerCtr = other.GetComponentInParent<PlayerBase>();
        if (playerCtr != null)
            _collidedWithPlayer = false;
    }

    void _Transfer()
    {
        MapMgr.Instance.Go(mapID);
        enabled = false;
    }

    void _HideEffect()
    {
        _effectActive.SetActive(false);
    }

}
