using UnityEngine;
using System.Collections;

public class ItemBoxUI : MonoBehaviour 
{
    public UISprite icon;
    public UISprite quality;

    public void Reset(enItemData data)
    {
        icon.spriteName = data.icon;
        quality.color = data.qualityColor;
    }
}
