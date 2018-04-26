using UnityEngine;
using System.Collections;

public class AutoLabCtr : MonoBehaviour 
{
    /// <summary>
    /// 最大行
    /// </summary>
    public int maxLine;
    /// <summary>
    /// 最大列
    /// </summary>
    public int maxColumn;
    /// <summary>
    /// 字体大小
    /// </summary>
    public float fontSize;
    string _factText;
    UILabel _lab;
    UILabel lab
    {
        get
        {
            if (_lab == null)
                _lab = GetComponentInChildren<UILabel>();
            return _lab;
        }
    }
	public void SetText(string content)
    {
        lab.text = content;
        _factText = content.Substring(1);
        _UpdateSize();
    }
    void _UpdateSize()
    {

    }
}
