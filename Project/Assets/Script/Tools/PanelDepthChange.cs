using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class PanelDepthChange : MonoBehaviour
{
    int _depth;
    public int depth;

    UIPanel _rootPanel;
    UIPanel _panel;

    void Awake()
    {
        _depth = 0;
        _rootPanel = transform.parent.GetComponentInParent<UIPanel>();
        _panel = GetComponent<UIPanel>();
    }

    void Update()
    {
        if (!Application.isPlaying && depth != _depth)
            _ChangeDepth();
    }

	void Start()
    {
        _ChangeDepth();
    }

    void _ChangeDepth()
    {
        if(_rootPanel != null && _panel != null)
            _panel.depth = _rootPanel.depth + depth;
        _depth = depth;
    }
}
