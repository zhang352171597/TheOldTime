using UnityEngine;
using System.Collections;

public class DropItemCtr : MonoBehaviour 
{
    public enItemGrade TestColor;

    /// <summary>
    /// 掉落动作持续时间
    /// </summary>
    const float DROPDURATION = 0.5f;
    /// <summary>
    /// 掉落随机半径最大值
    /// </summary>
    const float DROPRADIUS = 4;
    /// <summary>
    /// 掉落抛狐高度
    /// </summary>
    const float DROPHEIGHT = 4;
    /// <summary>
    /// 自转表现器
    /// </summary>
    RotBySelf _rotCtr;
    /// <summary>
    /// 掉落位置
    /// </summary>
    Vector3 _targetPos;
    /// <summary>
    /// 抛狐算法
    /// </summary>
    BeizerCurver _beizer;
    /// <summary>
    /// 掉落持续时间计时器
    /// </summary>
    float _dropDurationCount;
    /// <summary>
    /// 拾取回调
    /// </summary>
    System.Action _gainCallback;
    /// <summary>
    /// 销毁回调
    /// </summary>
    System.Action<DropItemCtr> _onDestroy;
    /// <summary>
    /// 道具数据
    /// </summary>
    enDropData.strDropResult _itemData;
    void Awake()
    {
        _fsm.AddState(_drop, _DropEnter, _DropUpdate, null);
        _fsm.AddState(_idle, _IdleEnter, null, null);
        _fsm.AddState(_gain, _GainEnter, _GainUpdate , null);
		_fsm.AddState (_destroy, null, null, null);
        _rotCtr = GetComponentInChildren<RotBySelf>();
    }
    public void Begin(enDropData.strDropResult item, Vector3 bornPos, System.Action gainCallback , System.Action<DropItemCtr> onDestroy)
    {
        _gainCallback = gainCallback;
        _onDestroy = onDestroy;
        transform.position = bornPos;
        _itemData = item;
        var tempDir = Random.insideUnitSphere;
        tempDir.y = 0;
        _targetPos = bornPos + tempDir * Random.Range(0, DROPRADIUS);
        var renderer = GetComponentInChildren<Renderer>();
        var targetColor = Contrastting.qualityColor[(int)JSONItem.getInstance().GetQuality(item.id)];
        for (int i = 0; i < renderer.materials.Length; ++i)
        {
            renderer.materials[i].SetColor("_RimColor", targetColor);
        }
        _fsm.SetState(_drop);
    }
	public void OnDespawn()
	{
		_fsm.SetState (_destroy);
	}
    void Update()
    {
        _fsm.Update(Time.deltaTime);
    }

    #region FSM
    FSM _fsm = new FSM();
    FSMState _drop = new FSMState("drop");
    FSMState _idle = new FSMState("idle");
    FSMState _gain = new FSMState("gain");
	FSMState _destroy = new FSMState("destroy");
    void _DropEnter()
    {
        _beizer = BeizerHelper.CalculateParabolaCurver(transform.position, _targetPos, Vector3.up, DROPHEIGHT / 2);
        _dropDurationCount = 0;
    }
    void _DropUpdate(float dt)
    {
        _dropDurationCount += dt;
        _dropDurationCount = _dropDurationCount > DROPDURATION ? DROPDURATION : _dropDurationCount;
        transform.position = _beizer.GetPosition(_dropDurationCount / DROPDURATION, true);
        if (transform.position == _targetPos)
            _fsm.SetState(_idle);
    }
    void _IdleEnter()
    {
        _rotCtr.isActive = true;
    }
    void _GainEnter()
    {
        _beizer = BeizerHelper.CalculateParabolaCurver(transform.position
            , transform.position, Vector3.up, DROPHEIGHT);
        _dropDurationCount = 0;
    }
    void _GainUpdate(float dt)
    {
        _dropDurationCount += dt;
        _dropDurationCount = _dropDurationCount > DROPDURATION ? DROPDURATION : _dropDurationCount;
        transform.position = _beizer.GetPosition(_dropDurationCount / DROPDURATION, true);
        if (_dropDurationCount == DROPDURATION)
        {
            DataCenter.Instance.backpackData.Add(_itemData.id, _itemData.count);
            if (_gainCallback != null)
                _gainCallback();
            if (_onDestroy != null)
                _onDestroy(this);
        }
    }
    #endregion

    void OnTriggerEnter(Collider other)
    {
        if (!_fsm.IsState(_idle))
            return;
        _fsm.SetState(_gain);
    }

}
