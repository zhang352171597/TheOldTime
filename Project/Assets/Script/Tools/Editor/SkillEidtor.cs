using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System;
using JSON;
using System.IO;

public class SkillEditor : EditorWindow 
{
	[MenuItem("GTool/-----牛逼轰轰的技能编辑器-----")]
	public static void Init(){
        var tempEditor = EditorWindow.GetWindow<SkillEditor>();
        tempEditor.Load();
	}

    const int UIBUTTONSACLE = 70;
    const int UIFIELDSACLE = 150;

	public void OnGUI()
    {
        if (_skillDatas == null)
            Load();
        EditorGUILayout.BeginHorizontal();
        _DrawSkillList();
        _DrawSkillEditor();
        EditorGUILayout.EndHorizontal();
	}

    enSkillDatasForEditor _skillDatas;
    int _currentSkillIndex;
    int _currentStateIndex;
    enSkillDataForEditor currentSkillData
    {
        get
        {
            if (_skillDatas.list != null &&_currentSkillIndex >= 0 && _currentSkillIndex < _skillDatas.list.Count)
                return _skillDatas.list[_currentSkillIndex];
            return null;
        }
    }

    string searchOnSkillList = "";
    Vector2 skilllistpos = new Vector2();
    void _DrawSkillList()
    {
        skilllistpos = EditorGUILayout.BeginScrollView(skilllistpos, GUILayout.MaxWidth(220));
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
        searchOnSkillList = EditorGUILayout.TextField(searchOnSkillList);
        GUILayout.EndHorizontal();

        for (int i = 0; i < _skillDatas.list.Count; i++) 
        {
            if (i == _currentSkillIndex)
                GUI.backgroundColor = Color.green;
            else
                GUI.backgroundColor = Color.white;

            if (isOtherLikeID(_skillDatas.list[i].id))
                GUI.backgroundColor = Color.red;

            if (_skillDatas.list[i].name == null)
                _skillDatas.list[i].name = "";

            if (((_skillDatas.list[i].name.ToString().Contains(searchOnSkillList))
                || (_skillDatas.list[i].id.ToString().Contains(searchOnSkillList)))
                && GUILayout.Button(_skillDatas.list[i].name + ":" + _skillDatas.list[i].id))
            {
                SelectSkill(i);
            }
            GUI.backgroundColor = Color.white;
        }
        EditorGUILayout.EndScrollView();
    }

    Vector2 skillStatePos = new Vector2();
    void _DrawSkillEditor()
    {
        EditorGUILayout.BeginVertical("box");
        if (currentSkillData == null)
            return;

        DrawValue("ID: ", ref currentSkillData.id);
        DrawValue("Name: ", ref currentSkillData.name);
        skillStatePos = EditorGUILayout.BeginScrollView(skillStatePos, "box");
        for (int i = 0; i < currentSkillData._states.Count; ++i)
        {
            if(_DrawStateEditor(i , currentSkillData._states[i]))
                break;
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    bool _DrawStateEditor(int index , enSkillStateForEditor stateData)
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
            currentSkillData.AddState();
        if (GUILayout.Button("-"))
            currentSkillData.RemoveState(stateData);

        if (index > 0 && GUILayout.Button("上移"))
        {
            _StateUp(index);
            return true;
        }
        if (index < currentSkillData._states.Count - 1 && GUILayout.Button("下移"))
        {
            _StateDown(index);
            return true;
        }
        EditorGUILayout.EndHorizontal();

        DrawValue("名称: ", ref stateData.name);
        DrawValue("图标: ", ref stateData.icon);
        DrawTextField("说明 ", ref stateData.info, 450, 150);
        DrawValue("冷却时间 ", ref stateData.coldTime);
        DrawValue("释放前摇，满足未释放进入CD ", ref stateData.beforeTime);
        DrawValue("冷却前摇，满足未切换进入CD", ref stateData.afterTime);
        DrawValue("装载时间，满足可释放 ", ref stateData.readyTime);
        if (DrawBtn("技能预制：" + stateData.prefabName, "设置"))
        {
            _currentStateIndex = index;
            EPrefabsTool.Init(_SelectPrefabs);
        }
        DrawValue("生成位置偏移", ref stateData.bornPosOffset);

        EditorGUILayout.EndVertical();
        return false;
    }

    #region DataOperation
    void Load()
    {
        _skillDatas = new enSkillDatasForEditor();
        _skillDatas.Load();
        if (_skillDatas.list.Count > 0)
            SelectSkill(0);
    }

    void Save()
    {
        _skillDatas.Save();
    }

    void Add()
    {
        _skillDatas.Add();
        SelectSkill(_skillDatas.list.Count - 1);
    }

    void Remove()
    {
        _skillDatas.Remove(_currentSkillIndex);
        SelectSkill(_skillDatas.list.Count - 1);
    }

    void Sort()
    {
        if (_skillDatas == null)
            return;

        ///冒了个泡 ---ps i和i之后所有的比 最小的放在i   然后 i++  以此类推
        for (int i = 0; i < _skillDatas.list.Count; ++i)
        {
            for (int j = i + 1; j < _skillDatas.list.Count; ++j)
            {
                if (int.Parse(_skillDatas.list[i].id) > int.Parse(_skillDatas.list[j].id))
                {
                    var temp = _skillDatas.list[i];
                    _skillDatas.list[i] = _skillDatas.list[j];
                    _skillDatas.list[j] = temp;
                }
            }
        }
    }

    /// <summary>
    /// 选择技能
    /// </summary>
    /// <param name="index"></param>
    void SelectSkill(int index)
    {
        _currentSkillIndex = index;
    }

    bool isOtherLikeID(string id)
    {
        int like = 0;
        for (int i = 0; i < _skillDatas.list.Count; i++)
        {
            if (_skillDatas.list[i].id == id)
                like++;
            if (like >= 2)
                return true;
        }
        return false;
    }

    void _StateUp(int index)
    {
        if (currentSkillData == null || index == 0)
            return;

        var p = currentSkillData._states[index];
        currentSkillData._states.Remove(p);
        currentSkillData._states.Insert(index - 1, p);
    }

    void _StateDown(int index)
    {
        if (currentSkillData == null || index >= _skillDatas.list.Count - 1)
            return;

        var p = currentSkillData._states[index];
        currentSkillData._states.Remove(p);
        currentSkillData._states.Insert(index + 1, p);
    }

    void _SelectPrefabs(int index)
    {
        currentSkillData._states[_currentStateIndex].prefabName = SkillEditorCommon.EffectPrefabs[index];
        Repaint();
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




#region 总技能数据类
/// <summary>
/// 总技能数据类
/// </summary>
public class enSkillDatasForEditor
{
    public List<enSkillDataForEditor> list = new List<enSkillDataForEditor>();
    JSONNode json;

    public void Load()
    {
        json = JsonRead.Read(SkillTag.JSONName)[JSONTag.root];
        list = new List<enSkillDataForEditor>();
        for (int i = 0; i < json.Count; i++)
        {
            list.Add(new enSkillDataForEditor(json[i]));
        }
    }
    public void Save()
    {
        StreamWriter writer;
        FileInfo file = new FileInfo(Application.dataPath + "/Resources/Data/" + SkillTag.JSONName + ".txt");
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
        var tempSkill = new enSkillDataForEditor();
        tempSkill.id = "??";
        tempSkill.name = "填写技能名称";
        list.Add(tempSkill);
    }

    public void Remove(int index)
    {
        if (index >= 0 && index < list.Count)
            list.RemoveAt(index);
    }
}
#endregion

#region 技能数据

public class enSkillDataForEditor
{
    JSONNode node;

    /// <summary>
    /// 名称
    /// </summary>
    public string name = "";

    /// <summary>
    /// 技能ID
    /// </summary>
    public string id = "";

    /// <summary>
    /// 技能状态集合
    /// </summary>
    public List<enSkillStateForEditor> _states = new List<enSkillStateForEditor>();

    public enSkillDataForEditor()
    {
        AddState();
    }

    public enSkillDataForEditor(JSONNode json)
    {
        node = json;
        id = getString(JSONTag.id);
        name = getString(JSONTag.name);
        var stateNodes = JSONNode.Parse(node[SkillTag.states]);
        for(int i = 0 ; i < stateNodes.Count; ++i)
        {
            _states.Add(new enSkillStateForEditor(stateNodes[i]));
        }
    }
    public string GetJsonData()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(JSONTag.id, id);
        data.Add(JSONTag.name, name);
        var states = new ArrJsonData();
        for (int i = 0; i < _states.Count; ++i)
        {
            states.Add(_states[i].GetJsonData());
        }
        data.Add(SkillTag.states, states.GetString());
        return data.GetString();
    }

    public void AddState()
    {
        _states.Add(new enSkillStateForEditor());
    }

    public void RemoveState(enSkillStateForEditor data)
    {
        _states.Remove(data);
    }

    string getString(string key)
    {
        var result = node[key] != null ? node[key].ToString() : "";
        return result;
    }
    int getInt(string key)
    {
        return node[key].AsInt;
    }
    float getFloat(string key)
    {
        return node[key].AsFloat;
    }
    bool getBool(string key)
    {
        return node[key].AsInt == 1;
    }
}

#endregion

#region 技能状态数据
public class enSkillStateForEditor
{
    JSONNode node;
    /// <summary>
    /// 名称
    /// </summary>
    public string name = "";
    /// <summary>
    /// 图标
    /// </summary>
    public string icon = "";
    /// <summary>
    /// 说明
    /// </summary>
    public string info = "";
    /// <summary>
    /// 剩余解冻时间 0时  冷却完毕
    /// </summary>
    public float coldTime = -1;
    /// <summary>
    /// 当技能冷却完毕之后 准备时间结束才可以释放
    /// </summary>
    public float readyTime = -1;
    /// <summary>
    /// 释放前段计时 --即技能可以释放时计时 计时结束技能将进入CD
    /// </summary>
    public float beforeTime = -1;
    /// <summary>
    /// 释放后段计时 --即技能释放完开始计时 不直接切换下一状态 计时结束技能将进入CD 
    /// </summary>
    public float afterTime = -1;
    /// <summary>
    /// 技能预制
    /// </summary>
    public string prefabName = "";
    /// <summary>
    /// 生成偏移
    /// </summary>
    public float bornPosOffset;

    public enSkillStateForEditor(){}

    public enSkillStateForEditor(JSONNode json)
    {
        try
        {
            node = json;
            name = getString(JSONTag.name);
            icon = getString(JSONTag.icon);
            info = getString(JSONTag.info);
            coldTime = getFloat(SkillTag.coldTime);
            readyTime = getFloat(SkillTag.readyTime);
            beforeTime = getFloat(SkillTag.beforeTime);
            afterTime = getFloat(SkillTag.afterTime);
            prefabName = getString(SkillTag.prefabName);
            bornPosOffset = getFloat(SkillTag.bornPosOffset);
        }
        catch
        {

        }
    }
    public string GetJsonData()
    {
        ListJsonData data = ListJsonData.Init();
        data.Add(JSONTag.name, name);
        data.Add(JSONTag.icon, icon);
        data.Add(JSONTag.info , info);
        data.Add(SkillTag.coldTime, coldTime);
        data.Add(SkillTag.readyTime, readyTime);
        data.Add(SkillTag.beforeTime, beforeTime);
        data.Add(SkillTag.afterTime, afterTime);
        data.Add(SkillTag.prefabName, prefabName);
        data.Add(SkillTag.bornPosOffset, bornPosOffset);
        return data.GetString();
    }

    string getString(string key)
    {
        var result = node[key] != null ? node[key].ToString() : "";
        return result;
    }
    int getInt(string key)
    {
        return node[key].AsInt;
    }
    float getFloat(string key)
    {
        return node[key].AsFloat;
    }
    bool getBool(string key)
    {
        return node[key].AsInt == 1;
    }
}
#endregion

#region 配置数据缓存

public class SkillEditorCommon
{
    public static string[] EffectPrefabs;
    public static void LoadEffectsPrefab()
    {
        EffectPrefabs = Directory.GetFiles(Application.dataPath + "/Resources/" + GamePath.SkillPrefabs);
        if (EffectPrefabs != null)
        {
            List<string> path = new List<string>();
            for (var i = 0; i < EffectPrefabs.Length; i++)
            {
                if (Path.GetExtension(EffectPrefabs[i]) == ".prefab")
                {
                    path.Add(Path.GetFileNameWithoutExtension(EffectPrefabs[i]));
                }
            }
            EffectPrefabs = path.ToArray();
        }
    }
}
#endregion


#region 工具对话框
public class EPrefabsTool : EditorWindow
{
    VoidDelegateInt callback;
    Vector2 viewpos;
    string search = "";
    public static void Init(VoidDelegateInt cb)
    {
        GetWindow<EPrefabsTool>().callback = cb;
    }
    void OnGUI()
    {
        if (SkillEditorCommon.EffectPrefabs == null)
            SkillEditorCommon.LoadEffectsPrefab();

        search = EditorGUILayout.TextField(search);
        viewpos = EditorGUILayout.BeginScrollView(viewpos);
        for (int i = 0; i < SkillEditorCommon.EffectPrefabs.Length; i++)
        {
            if (SkillEditorCommon.EffectPrefabs[i].Contains(search))
            {
                DrawBtns(i);
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void DrawBtns(int i)
    {
        EditorGUILayout.BeginHorizontal("box");
        EditorGUILayout.LabelField(SkillEditorCommon.EffectPrefabs[i]);
        if (GUILayout.Button("选择", GUILayout.Width(30)))
        {
            if (callback != null)
                callback(i);
            Close();
        }
        EditorGUILayout.EndHorizontal();
    }
}



#endregion