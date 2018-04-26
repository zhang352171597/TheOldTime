using UnityEngine;
using System.Collections;

/// <summary>
/// 背包列表单个元素控制器
/// </summary>
public class BackpackSingleItemCtr : MonoBehaviour 
{
    
    public UIDragScrollView dragView;
    public UISprite stateTip;
    public Transform itemAnchor;
    public UISprite icon;
    public UISprite grade;
    public UILabel count;
    public BackpackSingleItemCollider colliderCtr;
    enItemData _data;
	public void Begin(UIScrollView view)
    {
        dragView.scrollView = view;
    }
    public void ResetData(enItemData data)
    {
        _data = data;
        colliderCtr.ResetData(data);
        if (_data != null)
        {
            itemAnchor.gameObject.SetActive(data.count > 0);
            count.text = "" + data.count;
            count.gameObject.SetActive(data.count > 1);
            icon.spriteName = data.icon;
            grade.color = data.qualityColor;
        }
        else
            itemAnchor.gameObject.SetActive(false);
    }
}
