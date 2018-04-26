using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjManager : ModuleComponent<ObjManager> 
{
    Dictionary<string, ObjData> _dic = new Dictionary<string,ObjData>();

    /// <summary>
    /// 普通对象回收节点
    /// </summary>
    Transform _defaultParent;
    public Transform defaultParent
    {
        get 
        { 
            if(_defaultParent == null)
            {
                var obj = new GameObject("ObjManager_default");
                _defaultParent = obj.transform;
                _defaultParent.parent = transform;
            }
            return _defaultParent; 
        }
    }

    Transform _uiParent;
    /// <summary>
    /// UI对象回收节点
    /// </summary>
    public Transform uiParent
    {
        get
        {
            if (_uiParent == null)
            {
                var root = GameObject.FindObjectOfType<UIRoot>();
                if(root != null)
                {
                    var obj = new GameObject("ObjManager_ui");
                    obj.transform.parent = root.transform;
                    obj.AddComponent<UIWidget>();
                    _uiParent = obj.transform;
                }
            }
            return _uiParent;
        }
    }

    public GameObject addChild(string path , string name ,Transform parent, bool isUI = false)
    {
        GameObject targetObj = null;
        if (_dic.ContainsKey(name))
            targetObj = _dic[name].GetObj();
        else
        {
            var data = new ObjData(path, name , isUI);
            _dic.Add(name, data);
            targetObj = data.GetObj();
        }
        if(parent != null)
        {
            targetObj.transform.parent = parent;
            targetObj.transform.localPosition = Vector3.zero;
            targetObj.transform.localScale = Vector3.one;
        }
        return targetObj;
    }

    public GameObject addChild(GameObject obj, Transform parent, bool isUI = false)
    {
        GameObject targetObj = null;
        if (_dic.ContainsKey(obj.name))
            targetObj = _dic[obj.name].GetObj();
        else
        {
            var data = new ObjData(obj , isUI);
            _dic.Add(obj.name, data);
            targetObj = data.GetObj();
        }
        if (parent != null)
        {
            targetObj.transform.parent = parent;
            targetObj.transform.localPosition = Vector3.zero;
            targetObj.transform.localScale = Vector3.one;
        }
        return targetObj;
    }

    public void Despawn(GameObject obj , bool isUI = false)
    {
        if(_dic.ContainsKey(obj.name))
        {
            _dic[obj.name].Despawn(obj);
        }
        else
        {
            var data = new ObjData(obj, isUI);
            _dic.Add(obj.name, data);
            data.Despawn(obj);
        }
    }
}


public class ObjData
{
    static Vector3 DESPAWNPOS = new Vector3(-1000, 0, 0);

    /// <summary>
    /// 缓存预制
    /// </summary>
    GameObject prefabRes;
    /// <summary>
    /// 回收池
    /// </summary>
    List<GameObject> _pool;
    /// <summary>
    /// 使用池
    /// </summary>
    List<GameObject> _usingPool;
    /// <summary>
    /// 路径
    /// </summary>
    string _path;
    /// <summary>
    /// 名称
    /// </summary>
    string _name;
    /// <summary>
    /// 是否是UI
    /// </summary>
    bool _isUI;

    public ObjData(string path , string name , bool isUI)
    {
        _path = path;
        _name = name;
        _isUI = isUI;
        _pool = new List<GameObject>();
        _usingPool = new List<GameObject>();
    }

    public ObjData(GameObject obj , bool isUI)
    {
        prefabRes = obj;
        _name = obj.name;
        _isUI = isUI;
        _pool = new List<GameObject>();
        _usingPool = new List<GameObject>();
    }

    public GameObject GetObj()
    {
        _cheackExist();
        if(_pool.Count > 0)
        {
            var instance = _pool[0];
            _pool.Remove(instance);
            _usingPool.Add(instance);
            instance.SetActive(true);
            return instance;
        }
        else
        {
            try
            {
                var instance = GameObject.Instantiate(prefabRes);
                instance.transform.parent = _isUI ? ObjManager.Instance.uiParent : ObjManager.Instance.defaultParent;
                instance.name = _name;
                _usingPool.Add(instance);
                return instance;
            }
            catch
            {
                Debug.LogError("error load :" + _path + _name);
                return null;
            }
        }
    }

    public void Despawn(GameObject obj)
    {
        if (_usingPool.Contains(obj))
        {
            obj.SetActive(false);
            obj.transform.position = DESPAWNPOS;
            //UI父节点更改导致层级表现错误
            /*if (_isUI)
                obj.transform.parent = ObjManager.Instance.uiParent;*/
            //非UI专设移除节点避免删除临时节点把需要回收的资源也删除了
            if(!_isUI)
                obj.transform.parent = ObjManager.Instance.defaultParent;
            _usingPool.Remove(obj);
            _pool.Add(obj);
        }
        else
            GameObject.Destroy(obj);
    }

    void _cheackExist()
    {
        if (prefabRes == null)
        {
            try
            {
                prefabRes = Resources.Load(_path + _name) as GameObject;
            }
            catch { Debug.LogError(_path + _name + "is not exist!"); }
        }
    }
}