using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using JSON;
using System.IO;

public class BuffEditor : EditorWindow
{
    [MenuItem("GTool/-----牛逼轰轰的Buff编辑器-----")]
    public static void Init()
    {
        var tempEditor = EditorWindow.GetWindow<BuffEditor>();
        tempEditor.Load();
    }

    const int UIBUTTONSACLE = 70;
    const int UIFIELDSACLE = 150;

    enBuffDatasForEditor _buffDatas;
    int _currentBuffIndex;
    int _currentBuffClipIndex;
    enBuffDataForEditor currentBuffData
    {
        get
        {
            if (_buffDatas.list != null && _currentBuffIndex >= 0 && _currentBuffIndex < _buffDatas.list.Count)
                return _buffDatas.list[_currentBuffIndex];
            return null;
        }
    }


    public void OnGUI()
    {
        if (_buffDatas == null)
            Load();
        EditorGUILayout.BeginHorizontal();
        _DrawBuffList();
        _DrawBuffEditor();
        EditorGUILayout.EndHorizontal();
    }

    string searchOnBufflList = "";
    Vector2 bufflistpos = new Vector2();
    void _DrawBuffList()
    {
        bufflistpos = EditorGUILayout.BeginScrollView(bufflistpos, GUILayout.MaxWidth(220));
        EditorGUILayout.BeginHorizontal();
        DrawButton("保存", Save, Color.green);
        DrawButton("读取", Load, Color.red);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        DrawButton("添加", Add, Color.green);
        DrawButton("删除", Remove, Color.red);
        DrawButton("排序", Sort, Color.green);
        EditorGUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("搜索：");
        searchOnBufflList = EditorGUILayout.TextField(searchOnBufflList);
        GUILayout.EndHorizontal();

        for (int i = 0; i < _buffDatas.list.Count; i++)
        {
            if (i == _currentBuffIndex)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.white;

            if (isOtherLikeID(_buffDatas.list[i].id))
                GUI.backgroundColor = Color.red;

            if (_buffDatas.list[i].name == null)
                _buffDatas.list[i].name = "";

            if (((_buffDatas.list[i].name.ToString().Contains(searchOnBufflList))
                || (_buffDatas.list[i].id.ToString().Contains(searchOnBufflList)))
                && GUILayout.Button(_buffDatas.list[i].name + ":" + _buffDatas.list[i].id))
            {
                SelectSkill(i);
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();
    }

    Vector2 buffeditorpos = new Vector2();
    void _DrawBuffEditor()
    {
        EditorGUILayout.BeginVertical("box");
        if (currentBuffData == null)
            return;

        DrawValue("ID: ", ref currentBuffData.id);
        DrawValue("Name: ", ref currentBuffData.name);
        currentBuffData.type = (int)(enBuffType)DrawPop("类型", (System.Enum)(enBuffType)currentBuffData.type);
        currentBuffData.functionType = (int)(enBuffFunctionType)DrawPop("类型", (System.Enum)(enBuffFunctionType)currentBuffData.functionType);
        if (currentBuffData.type != (int)enBuffType.瞬间)
            DrawValue("持续时间", ref currentBuffData.duration);
        if (currentBuffData.type == (int)enBuffType.持续)
            DrawValue("频率", ref currentBuffData.frequency);
        buffeditorpos = EditorGUILayout.BeginScrollView(buffeditorpos, "box");
        for (int i = 0; i < currentBuffData._clips.Count; ++i)
        {
            if (_DrawClipEditor(i, currentBuffData._clips[i]))
                break;
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }


    bool _DrawClipEditor(int index, enBuffClipForEditor clipData)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
            currentBuffData.AddState();
        if (currentBuffData._clips.Count > 1 && GUILayout.Button("-"))
            currentBuffData.RemoveState(clipData);

        if (index > 0 && GUILayout.Button("上移"))
        {
            _StateUp(index);
            return true;
        }
        if (index < currentBuffData._clips.Count - 1 && GUILayout.Button("下移"))
        {
            _StateDown(index);
            return true;
        }
        EditorGUILayout.EndHorizontal();


        clipData.workType = (int)(enBuffReferType)DrawPop("作用方", (System.Enum)(enBuffReferType)clipData.workType);
        clipData.workTagType = (int)(enBuffAttTargetTag)DrawPop("作用数值标识", (System.Enum)(enBuffAttTargetTag)clipData.workTagType);
        clipData.coreValueType = (int)(enBuffCoreValueType)DrawPop("作用数值标识", (System.Enum)(enBuffCoreValueType)clipData.coreValueType);
        if (clipData.coreValueType == (int)enBuffCoreValueType.固定值)
            DrawValue("核心值： " , ref clipData.coreValue);
        else
        {
            clipData.referType = (int)(enBuffReferType)DrawPop("取值参照方", (System.Enum)(enBuffReferType)clipData.referType);
            clipData.referTagType = (int)(enBuffAttTargetTag)DrawPop("取值参照标识", (System.Enum)(enBuffAttTargetTag)clipData.referTagType);
            GUILayout.BeginHorizontal();
            DrawValue("核心值： ", ref clipData.coreValue);
            DrawLab("%", "");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            DrawValue("最小值： ", ref clipData.minValue);
            DrawValue("最大值： ", ref clipData.maxValue);
            GUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();
        return false;
    }
    #region DataOperation
    void Load()
    {
        _buffDatas = new enBuffDatasForEditor();
        _buffDatas.Load();
        if (_buffDatas.list.Count > 0)
            SelectSkill(0);
    }

    void Save()
    {
        _buffDatas.Save();
    }

    void Add()
    {
        _buffDatas.Add();
        SelectSkill(_buffDatas.list.Count - 1);
    }

    void Remove()
    {
        _buffDatas.Remove(_currentBuffIndex);
        SelectSkill(_buffDatas.list.Count - 1);
    }

    void Sort()
    {
        if (_buffDatas == null)
            return;

        ///冒了个泡 ---ps i和i之后所有的比 最小的放在i   然后 i++  以此类推
        for (int i = 0; i < _buffDatas.list.Count; ++i)
        {
            for (int j = i + 1; j < _buffDatas.list.Count; ++j)
            {
                if (int.Parse(_buffDatas.list[i].id) > int.Parse(_buffDatas.list[j].id))
                {
                    var temp = _buffDatas.list[i];
                    _buffDatas.list[i] = _buffDatas.list[j];
                    _buffDatas.list[j] = temp;
                }
            }
        }
    }

    bool isOtherLikeID(string id)
    {
        int like = 0;
        for (int i = 0; i < _buffDatas.list.Count; i++)
        {
            if (_buffDatas.list[i].id == id)
                like++;
            if (like >= 2)
                return true;
        }
        return false;
    }

    void _StateUp(int index)
    {
        if (currentBuffData == null || index == 0)
            return;

        var p = currentBuffData._clips[index];
        currentBuffData._clips.Remove(p);
        currentBuffData._clips.Insert(index - 1, p);
    }

    void _StateDown(int index)
    {
        if (currentBuffData == null || index >= _buffDatas.list.Count - 1)
            return;

        var p = currentBuffData._clips[index];
        currentBuffData._clips.Remove(p);
        currentBuffData._clips.Insert(index + 1, p);
    }

    void SelectSkill(int index)
    {
        _currentBuffIndex = index;
    }
    #endregion

    #region Tools
    bool DrawBtn(string lab, string btnname)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        bool value = false;
        if (GUILayout.Button(btnname, GUILayout.Width(UIBUTTONSACLE)))
            value = true;
        EditorGUILayout.EndHorizontal();
        return value;
    }
    System.Enum DrawPop(string lab, System.Enum _enum)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        _enum = EditorGUILayout.EnumPopup(_enum, GUILayout.Width(UIBUTTONSACLE));
        EditorGUILayout.EndHorizontal();
        return _enum;
    }

    System.Enum DrawPop(string lab, System.Enum _enum, float LabWidth, float buttonWidth)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab, GUILayout.Width(LabWidth));
        _enum = EditorGUILayout.EnumPopup(_enum, GUILayout.Width(buttonWidth));
        EditorGUILayout.EndHorizontal();
        return _enum;
    }

    void DrawButton(string lab, Action callback, Color color, int btnsacle = UIBUTTONSACLE)
    {
        GUI.backgroundColor = color;
        if (GUILayout.Button(lab, GUILayout.MaxWidth(btnsacle)))
            callback();
        GUI.backgroundColor = Color.white;
    }

    int DrawValue(string lab, ref int value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        value = EditorGUILayout.IntField(value, GUILayout.MaxWidth(UIFIELDSACLE));
        EditorGUILayout.EndHorizontal();
        return value;
    }
    string DrawValue(string lab, ref string value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        value = EditorGUILayout.TextField(value, GUILayout.MaxWidth(UIFIELDSACLE));
        EditorGUILayout.EndHorizontal();
        return value;
    }
    float DrawValue(string lab, ref float value)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        value = EditorGUILayout.FloatField(value, GUILayout.MaxWidth(UIFIELDSACLE));
        EditorGUILayout.EndHorizontal();
        return value;
    }
    string DrawTextField(string lab, ref string value, float maxWidth, float maxHeight)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab);
        value = EditorGUILayout.TextField(value, GUILayout.MaxWidth(maxWidth), GUILayout.MaxHeight(maxHeight));
        EditorGUILayout.EndHorizontal();
        return value;
    }

    void DrawLab(string lab, string value)
    {
        EditorGUILayout.LabelField(lab + ":" + value, GUILayout.MaxWidth(UIFIELDSACLE));
    }

    int DrawBool(string lab, ref int value, float LabWidth = UIFIELDSACLE)
    {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(lab, GUILayout.Width(LabWidth));
        value = EditorGUILayout.Toggle(value == 1, GUILayout.MaxWidth(UIFIELDSACLE)) == true ? 1 : 0;
        EditorGUILayout.EndHorizontal();
        return value;
    }

    void _DrawLine(Vector3 startPos, Vector3 endPos, Color color, float width = 3)
    {
        /*var halfPos = (endPos + startPos) / 2;
        //Handles.DrawBezier(startPos, halfPos, halfPos, endPos, color, null, width);
        Handles.color = color;
        Handles.DrawLine(startPos, endPos);
        Handles.color = Color.white;*/
    }
    #endregion
}


#region 总Buff数据类
/// <summary>
/// 总技能数据类
/// </summary>
public class enBuffDatasForEditor
{
    public List<enBuffDataForEditor> list = new List<enBuffDataForEditor>();
    JSONNode json;

    public void Load()
    {
        json = JsonRead.Read(BuffTag.JSONName)[JSONTag.root];
        list = new List<enBuffDataForEditor>();
        for (int i = 0; i < json.Count; i++)
        {
            list.Add(new enBuffDataForEditor(json[i]));
        }
    }
    public void Save()
    {
        StreamWriter writer;
        FileInfo file = new FileInfo(Application.dataPath + "/Resources/Data/" + BuffTag.JSONName + ".txt");
        if (file.Exists)
        {
            file.Delete();
        }
        writer = file.CreateText();
        writer.WriteLine("{");
        writer.WriteLine("\t\"" + JSONTag.root + "\":[");
        for (int i = 0; i < list.Count; i++)
        {
            writer.WriteLine(list[i].GetJsonData() + ",");
        }
        writer.WriteLine("\t]");
        writer.WriteLine("}");
        writer.Close();
        AssetDatabase.Refresh();
    }

    public void Add()
    {
        var tempSkill = new enBuffDataForEditor();
        tempSkill.id = "??";
        tempSkill.name = "填写buff名称";
        list.Add(tempSkill);
    }

    public void Remove(int index)
    {
        if (index >= 0 && index < list.Count)
            list.RemoveAt(index);
    }
}
#endregion

#region Buff数据

public class enBuffDataForEditor
{
    /// <summary>
    /// 名称
    /// </summary>
    public string name = "";

    /// <summary>
    /// 技能ID
    /// </summary>
    public string id = "";

    /// <summary>
    /// 判定类型
    /// </summary>
    public int type;

    /// <summary>
    /// 功能类型
    /// </summary>
    public int functionType;

    /// <summary>
    /// 持续时间
    /// </summary>
    public float duration;

    /// <summary>
    /// 频率
    /// </summary>
    public float frequency;

    /// <summary>
    /// buff剪辑
    /// </summary>
    public List<enBuffClipForEditor> _clips = new List<enBuffClipForEditor>();

    public enBuffDataForEditor()
    {
        AddState();
    }

    public enBuffDataForEditor(JSONNode json)
    {
        id = getString(json , JSONTag.id);
        name = getString(json, JSONTag.name);
        type = getInt(json, BuffTag.type);
        functionType = getInt(json, BuffTag.functionType);
        duration = getFloat(json, BuffTag.duration);
        frequency = getFloat(json, BuffTag.frequency);
        var clipNodes = JSONNode.Parse(json[BuffTag.buffClips]);
        for (int i = 0; i < clipNodes.Count; ++i)
        {
            _clips.Add(new enBuffClipForEditor(clipNodes[i]));
        }
    }

    public string GetJsonData()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(JSONTag.id, id);
        data.Add(JSONTag.name, name);
        data.Add(BuffTag.type, type);
        data.Add(BuffTag.functionType, functionType);
        data.Add(BuffTag.duration, duration);
        data.Add(BuffTag.frequency, frequency);
        var clips = new ArrJsonData();
        for (int i = 0; i < _clips.Count; ++i)
        {
            clips.Add(_clips[i].GetJsonData());
        }
        data.Add(BuffTag.buffClips, clips.GetString());
        return data.GetString();
    }

    public void AddState()
    {
        _clips.Add(new enBuffClipForEditor());
    }

    public void RemoveState(enBuffClipForEditor data)
    {
        _clips.Remove(data);
    }

    string getString(JSONNode node, string key)
    {
        var result = node[key] != null ? node[key].ToString() : "";
        return result;
    }
    int getInt(JSONNode node, string key)
    {
        return node[key].AsInt;
    }
    float getFloat(JSONNode node, string key)
    {
        return node[key].AsFloat;
    }
    bool getBool(JSONNode node, string key)
    {
        return node[key].AsInt == 1;
    }
}

#endregion


#region Buff剪辑数据解析类
public class enBuffClipForEditor
{
    /// <summary>
    /// 取值参照方
    /// </summary>
    public int referType;
    /// <summary>
    /// 取值参照标识
    /// </summary>
    public int referTagType;
    /// <summary>
    /// 作用方
    /// </summary>
    public int workType;
    /// <summary>
    /// 作用数值标识
    /// </summary>
    public int workTagType;
    /// <summary>
    /// 核心数值类型
    /// </summary>
    public int coreValueType;
    /// <summary>
    /// 核心作用值
    /// </summary>
    public float coreValue;
    /// <summary>
    /// 最小固定值
    /// </summary>
    public int minValue;
    /// <summary>
    /// 最大固定值
    /// </summary>
    public int maxValue;
    public enBuffClipForEditor()
    {

    }

    public enBuffClipForEditor(JSONNode node)
    {
        referType = node[BuffTag.referType].AsInt;
        referTagType = node[BuffTag.referTagType].AsInt;
        workType = node[BuffTag.workType].AsInt;
        workTagType = node[BuffTag.workTagType].AsInt;
        coreValueType = node[BuffTag.coreValueType].AsInt;
        coreValue = node[BuffTag.coreValue].AsFloat;
        minValue = node[BuffTag.minValue].AsInt;
        maxValue = node[BuffTag.maxValue].AsInt;
    }

    public string GetJsonData()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(BuffTag.referType, referType);
        data.Add(BuffTag.referTagType, referTagType);
        data.Add(BuffTag.workType, workType);
        data.Add(BuffTag.workTagType, workTagType);
        data.Add(BuffTag.coreValueType, coreValueType);
        data.Add(BuffTag.coreValue, coreValue);
        data.Add(BuffTag.minValue, minValue);
        data.Add(BuffTag.maxValue, maxValue);
        return data.GetString();
    }
}
#endregion